using System.ComponentModel.DataAnnotations;

namespace BlobStorageSample.Models
{
    /// <summary>
    /// アップロードファイル用モデル
    /// ※アップロードされたファイルはIFormFile型として送信される
    /// </summary>
    public class UploadModel
    {
        [UploadFile]
        public IFormFile? File { get; set; }
    }

    /// <summary>
    /// アップロードファイル検証用のカスタム属性
    /// </summary>
    public class UploadFileAttribute : ValidationAttribute
    {
        // 許可するファイルサイズ上限（今回は2MBを指定）
        private readonly long fileSizeLimit = 2097152;

        // 許可する拡張子
        private readonly string[] permittedExtensions = [".jpg", ".jpeg", ".png", ".gif"];

        // 検証メソッド
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // UploadModel型に変換
            var upload = validationContext.ObjectInstance as UploadModel;

            // ファイルが存在しない場合はエラー
            if (upload?.File == null)
            {
                return new ValidationResult("ファイルを選択してください。");
            }

            // ファイルサイズ上限より大きい場合はエラー
            if (upload.File.Length > fileSizeLimit)
            {
                return new ValidationResult("ファイルサイズが上限を超えています。");
            }

            // 小文字に変換したファイルの拡張子を取得
            var extension = Path.GetExtension(upload.File.FileName).ToLowerInvariant();

            // 画像の拡張子でない場合はエラー
            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
            {
                return new ValidationResult("画像ファイルを選択してください。");
            }

            return ValidationResult.Success;
        }
    }
}
