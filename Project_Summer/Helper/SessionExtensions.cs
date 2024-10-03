using System.Text.Json;

namespace Project_Summer.Helper
{
    public static class  SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value)); // lưu trữ dữ liệu của 1 đối tượng T dưới dạng chuỗi json vào session với key để nhận diện
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);// lấy dữ liẹue(value) từ session với khóa key và chuyển dữ liệu dạng json nhận được thành kiểu dữ liệu của dối tượng T
        }
    }
}
