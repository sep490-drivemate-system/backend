using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceService.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Update_Post_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_post_comments_posts_post_id",
                table: "post_comments");

            migrationBuilder.DropForeignKey(
                name: "FK_post_images_posts_post_id",
                table: "post_images");

            migrationBuilder.DropForeignKey(
                name: "FK_post_reactions_posts_post_id",
                table: "post_reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_post_reviews_posts_post_id",
                table: "post_reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_post_videos_posts_post_id",
                table: "post_videos");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_tags_TagId",
                table: "posts");

            migrationBuilder.DropForeignKey(
                name: "FK_qa_answers_qa_questions_question_id",
                table: "qa_answers");

            migrationBuilder.DropForeignKey(
                name: "FK_QaQuestionTag_qa_questions_QaQuestionsId",
                table: "QaQuestionTag");

            migrationBuilder.DropForeignKey(
                name: "FK_QaQuestionTag_tags_TagsId",
                table: "QaQuestionTag");

            migrationBuilder.DropTable(
                name: "post_categories");

            migrationBuilder.DropTable(
                name: "post_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tags",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_posts",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_TagId",
                table: "posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_qa_questions",
                table: "qa_questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_qa_answers",
                table: "qa_answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post_videos",
                table: "post_videos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post_reviews",
                table: "post_reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post_reactions",
                table: "post_reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post_images",
                table: "post_images");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "posts");

            migrationBuilder.RenameTable(
                name: "tags",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "posts",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "qa_questions",
                newName: "QaQuestions");

            migrationBuilder.RenameTable(
                name: "qa_answers",
                newName: "QaAnswers");

            migrationBuilder.RenameTable(
                name: "post_videos",
                newName: "PostVideos");

            migrationBuilder.RenameTable(
                name: "post_reviews",
                newName: "PostReviews");

            migrationBuilder.RenameTable(
                name: "post_reactions",
                newName: "PostReactions");

            migrationBuilder.RenameTable(
                name: "post_images",
                newName: "PostImages");

            migrationBuilder.RenameIndex(
                name: "IX_tags_slug",
                table: "Tags",
                newName: "IX_Tags_slug");

            migrationBuilder.RenameIndex(
                name: "IX_qa_answers_question_id",
                table: "QaAnswers",
                newName: "IX_QaAnswers_question_id");

            migrationBuilder.RenameIndex(
                name: "IX_post_videos_post_id",
                table: "PostVideos",
                newName: "IX_PostVideos_post_id");

            migrationBuilder.RenameIndex(
                name: "IX_post_reviews_post_id",
                table: "PostReviews",
                newName: "IX_PostReviews_post_id");

            migrationBuilder.RenameIndex(
                name: "IX_post_reactions_post_id_user_id",
                table: "PostReactions",
                newName: "IX_PostReactions_post_id_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_post_images_post_id",
                table: "PostImages",
                newName: "IX_PostImages_post_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QaQuestions",
                table: "QaQuestions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QaAnswers",
                table: "QaAnswers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostVideos",
                table: "PostVideos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReviews",
                table: "PostReviews",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReactions",
                table: "PostReactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostImages",
                table: "PostImages",
                column: "id");

            migrationBuilder.CreateTable(
                name: "CategoryPost",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPost", x => new { x.CategoriesId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_CategoryPost_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPost_PostsId",
                table: "CategoryPost",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsId",
                table: "PostTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_post_comments_Posts_post_id",
                table: "post_comments",
                column: "post_id",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_Posts_post_id",
                table: "PostImages",
                column: "post_id",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_Posts_post_id",
                table: "PostReactions",
                column: "post_id",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReviews_Posts_post_id",
                table: "PostReviews",
                column: "post_id",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostVideos_Posts_post_id",
                table: "PostVideos",
                column: "post_id",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QaAnswers_QaQuestions_question_id",
                table: "QaAnswers",
                column: "question_id",
                principalTable: "QaQuestions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QaQuestionTag_QaQuestions_QaQuestionsId",
                table: "QaQuestionTag",
                column: "QaQuestionsId",
                principalTable: "QaQuestions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QaQuestionTag_Tags_TagsId",
                table: "QaQuestionTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_post_comments_Posts_post_id",
                table: "post_comments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_Posts_post_id",
                table: "PostImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_Posts_post_id",
                table: "PostReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReviews_Posts_post_id",
                table: "PostReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_PostVideos_Posts_post_id",
                table: "PostVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_QaAnswers_QaQuestions_question_id",
                table: "QaAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QaQuestionTag_QaQuestions_QaQuestionsId",
                table: "QaQuestionTag");

            migrationBuilder.DropForeignKey(
                name: "FK_QaQuestionTag_Tags_TagsId",
                table: "QaQuestionTag");

            migrationBuilder.DropTable(
                name: "CategoryPost");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QaQuestions",
                table: "QaQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QaAnswers",
                table: "QaAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostVideos",
                table: "PostVideos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReviews",
                table: "PostReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReactions",
                table: "PostReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostImages",
                table: "PostImages");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "tags");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "posts");

            migrationBuilder.RenameTable(
                name: "QaQuestions",
                newName: "qa_questions");

            migrationBuilder.RenameTable(
                name: "QaAnswers",
                newName: "qa_answers");

            migrationBuilder.RenameTable(
                name: "PostVideos",
                newName: "post_videos");

            migrationBuilder.RenameTable(
                name: "PostReviews",
                newName: "post_reviews");

            migrationBuilder.RenameTable(
                name: "PostReactions",
                newName: "post_reactions");

            migrationBuilder.RenameTable(
                name: "PostImages",
                newName: "post_images");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_slug",
                table: "tags",
                newName: "IX_tags_slug");

            migrationBuilder.RenameIndex(
                name: "IX_QaAnswers_question_id",
                table: "qa_answers",
                newName: "IX_qa_answers_question_id");

            migrationBuilder.RenameIndex(
                name: "IX_PostVideos_post_id",
                table: "post_videos",
                newName: "IX_post_videos_post_id");

            migrationBuilder.RenameIndex(
                name: "IX_PostReviews_post_id",
                table: "post_reviews",
                newName: "IX_post_reviews_post_id");

            migrationBuilder.RenameIndex(
                name: "IX_PostReactions_post_id_user_id",
                table: "post_reactions",
                newName: "IX_post_reactions_post_id_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_PostImages_post_id",
                table: "post_images",
                newName: "IX_post_images_post_id");

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                table: "posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tags",
                table: "tags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_posts",
                table: "posts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_qa_questions",
                table: "qa_questions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_qa_answers",
                table: "qa_answers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_videos",
                table: "post_videos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_reviews",
                table: "post_reviews",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_reactions",
                table: "post_reactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_images",
                table: "post_images",
                column: "id");

            migrationBuilder.CreateTable(
                name: "post_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_post_categories_Category_category_id",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_categories_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_post_tags_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_posts_TagId",
                table: "posts",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_post_categories_category_id",
                table: "post_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_categories_post_id_category_id",
                table: "post_categories",
                columns: new[] { "post_id", "category_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_post_tags_post_id_tag_id",
                table: "post_tags",
                columns: new[] { "post_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_post_tags_tag_id",
                table: "post_tags",
                column: "tag_id");

            migrationBuilder.AddForeignKey(
                name: "FK_post_comments_posts_post_id",
                table: "post_comments",
                column: "post_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_images_posts_post_id",
                table: "post_images",
                column: "post_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_reactions_posts_post_id",
                table: "post_reactions",
                column: "post_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_reviews_posts_post_id",
                table: "post_reviews",
                column: "post_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_videos_posts_post_id",
                table: "post_videos",
                column: "post_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_tags_TagId",
                table: "posts",
                column: "TagId",
                principalTable: "tags",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_qa_answers_qa_questions_question_id",
                table: "qa_answers",
                column: "question_id",
                principalTable: "qa_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QaQuestionTag_qa_questions_QaQuestionsId",
                table: "QaQuestionTag",
                column: "QaQuestionsId",
                principalTable: "qa_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QaQuestionTag_tags_TagsId",
                table: "QaQuestionTag",
                column: "TagsId",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
