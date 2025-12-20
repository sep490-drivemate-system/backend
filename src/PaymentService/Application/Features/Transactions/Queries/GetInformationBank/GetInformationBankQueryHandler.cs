using MediatR;
using PaymentService.Application.Common.DTOs;
using System.Net.Http.Json;

namespace PaymentService.Application.Features.Transactions.Queries.GetInformationBank
{
    public class GetInformationBankQueryHandler
        : IRequestHandler<GetInformationBankQuery, VietQrLookupResponse>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GetInformationBankQueryHandler(
            HttpClient httpClient,
            IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<VietQrLookupResponse> Handle(
            GetInformationBankQuery request,
            CancellationToken cancellationToken)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add(
                "x-client-id", _config["VIETQR:CLIENTID"]);
            _httpClient.DefaultRequestHeaders.Add(
                "x-api-key", _config["VIETQR:APIKEY"]);

            var response = await _httpClient.PostAsJsonAsync(
                $"{_config["VIETQR:BASEURL"]}/v2/lookup",
                new GetWithdrawRequestQuery
                {
                    Bin = request.Bin,
                    AccountNumber = request.AccountNumber
                },
                cancellationToken
            );

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<VietQrLookupResponse>(cancellationToken);
        }
    }
}
