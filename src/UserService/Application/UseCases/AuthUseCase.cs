using AutoMapper;
using Microsoft.EntityFrameworkCore.Design;
using SharedLibrary.Email;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Password;
using SharedLibrary.SharedKernel.ServiceResult;
using SharedLibrary.Sms;
using System.Numerics;
using UserService.Application.Commons.Constants;
using UserService.Application.Commons.DTOs.Auth;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.UseCases
{
    public class AuthUseCase : IAuthUseCase
    {

        public IUnitOfWork _unitOfWork;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly ISmsService _smsService;
        private readonly IMapper _mapper;

        public AuthUseCase(IUnitOfWork unitOfWork, IPasswordHasherService passwordHasherService, IEmailService emailService, ISmsService smsService, IMapper mapper, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasherService = passwordHasherService;
            _emailService = emailService;
            _jwtService = jwtService;
            _smsService = smsService;
            _mapper = mapper;
        }

        // implement usecase verfyEmail
        public async Task<Result<string>> VerifyEmail(string email)
        {
            // 1. check exist email
            var isEmailExist = await _unitOfWork.UserRepository.IsEsxitEmail(email);
            if (isEmailExist)
            {
                return Result<string>.Failure(
                new ServiceError(ServiceError.Existed, Messages.Auth.EmailAlreadyExists)
            );
            }
            // 2. Generate code
            var codeGenerate = await _passwordHasherService.GenerateSecureVerificationCode();
            // 3. send email           
            var result = await _emailService.SendVerificationCodeAsync(email, codeGenerate);
            if (result)
            {
                return Result<string>.Success(codeGenerate, Messages.Auth.EmailSentSuccess);
            }
            return Result<string>.Failure(
                new ServiceError(ServiceError.Unhandled, Messages.Common.EmailError));
        }

        // implement usecase verify phone
        public async Task<Result<string>> VerifyPhone(string phone)
        {
            // 1. check exist phone number
            var isEmailExist = await _unitOfWork.UserRepository.IsEsxitPhone(phone);
            if (isEmailExist)
            {
                return Result<string>.Failure(
                new ServiceError(ServiceError.Existed, Messages.Auth.PhoneAlreadyExists)
            );
            }
            // 2. Generate code
            var codeGenerate = await _passwordHasherService.GenerateSecureVerificationCode();
            // 3. send code through sms
            var result = await _smsService.SendVerificationCodeAsync(phone, codeGenerate);
            if (result)
            {
                return Result<string>.Success(codeGenerate, Messages.Auth.SmsSentSuccess);
            }
            return Result<string>.Failure(
                new ServiceError(ServiceError.Unhandled, Messages.Common.PhoneError));
        }

        // implememt usecase signup 
        public async Task<Result<SignUpRespondDTO>> SignUp(SignUpDTO signUpDTO)
        {
            // 1.Check User Name input data
            var isUserNameExist = await _unitOfWork.UserRepository.IsEsxitUserName(signUpDTO.UserName);
            if (isUserNameExist)
            {
                return Result<SignUpRespondDTO>.Failure(
                new ServiceError(ServiceError.Existed, Messages.Auth.UserNameAlreadyExists));
            }

            var isPhoneExist = await _unitOfWork.UserRepository.IsEsxitPhone(signUpDTO.PhoneNumber);
            if (isUserNameExist)
            {
                return Result<SignUpRespondDTO>.Failure(
                new ServiceError(ServiceError.Existed, Messages.Auth.PhoneAlreadyExists));
            }


            // 2. Generate hash password and save data
            var hashedPassword = await _passwordHasherService.HashPassword(signUpDTO.Password);

            // 3. Map and save information
            var userMap = _mapper.Map<User>(signUpDTO);
            userMap.HashedPassword = hashedPassword;

            var user = await _unitOfWork.UserRepository.CreateAsync(userMap);
            await _unitOfWork.CommitChangesAsync();
            var userClaimToken = _mapper.Map<UserClaimTokenDTO>(user);
            var token = await _jwtService.GenerateAccessToken(userClaimToken);           


            // 4. Provide token(assign into responde)


            var respond = _mapper.Map<SignUpRespondDTO>(token);

            return Result<SignUpRespondDTO>.Success(respond);

        }

        // implement usecase signin
        public async Task<Result<SignInRespondDTO>> SignIn(SignInDTO signInDTO)
        {
            //1. Check user
            var user = await _unitOfWork.UserRepository.IsExistUser(signInDTO.EmailOrPhone);
           
            if (user == null)
            {
                return Result<SignInRespondDTO>.Failure(
                new ServiceError(ServiceError.NotFound, Messages.Auth.UserNorExists));
            }

            var isValidPassword = await _passwordHasherService.VerifyPassword(user.HashedPassword, signInDTO.Password);

            if (!isValidPassword)
            {
                return Result<SignInRespondDTO>.Failure(
               new ServiceError(ServiceError.NotFound, Messages.Auth.WrongPassword));
            }
            var userClaimToken = _mapper.Map<UserClaimTokenDTO>(user);
            var accessToken = await _jwtService.GenerateAccessToken(userClaimToken);



          
            var respond = _mapper.Map<SignInRespondDTO>((accessToken, accessToken));
            return Result<SignInRespondDTO>.Success(respond);

        }

      
    }
}
