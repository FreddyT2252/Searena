using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SEARENA2025
{
    internal static class DatabaseHelper
    {
        private static readonly string connectionString = "Host=aws-1-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.eeqqiyfukvhbwystupei;Password=SearenaDB123";

        public static NpgsqlConnection GetConnection()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Koneksi database gagal: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static void InitializeDatabase()
        {
            using (var conn = GetConnection())
            {
                if (conn == null) return;

                string createTables = @"
                    -- Table Users
                    CREATE TABLE IF NOT EXISTS users (
                        user_id SERIAL PRIMARY KEY,
                        nama_lengkap VARCHAR(100) NOT NULL,
                        email VARCHAR(100) UNIQUE NOT NULL,
                        password VARCHAR(255) NOT NULL,
                        no_telepon VARCHAR(20),
                        tanggal_bergabung TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        foto_profil TEXT
                    );

                    -- Table Destinasi
                    CREATE TABLE IF NOT EXISTS destinasi (
                        destinasi_id SERIAL PRIMARY KEY,
                        nama_destinasi VARCHAR(200) NOT NULL,
                        lokasi VARCHAR(200) NOT NULL,
                        pulau VARCHAR(50) NOT NULL,
                        deskripsi TEXT,
                        harga_min DECIMAL(10,2),
                        harga_max DECIMAL(10,2),
                        waktu_terbaik VARCHAR(100),
                        gambar_url TEXT,
                        rating_avg DECIMAL(3,2) DEFAULT 0,
                        total_review INT DEFAULT 0
                    );

                    -- Table Aktivitas
                    CREATE TABLE IF NOT EXISTS aktivitas (
                        aktivitas_id SERIAL PRIMARY KEY,
                        destinasi_id INT REFERENCES destinasi(destinasi_id) ON DELETE CASCADE,
                        nama_aktivitas VARCHAR(100) NOT NULL
                    );

                    -- Table Reviews
                    CREATE TABLE IF NOT EXISTS reviews (
                        review_id SERIAL PRIMARY KEY,
                        user_id INT REFERENCES users(user_id) ON DELETE CASCADE,
                        destinasi_id INT REFERENCES destinasi(destinasi_id) ON DELETE CASCADE,
                        rating INT CHECK (rating >= 1 AND rating <= 5),
                        ulasan TEXT,
                        tanggal_review TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    );

                    -- Table Bookmarks
                    CREATE TABLE IF NOT EXISTS bookmarks (
                        bookmark_id SERIAL PRIMARY KEY,
                        user_id INT REFERENCES users(user_id) ON DELETE CASCADE,
                        destinasi_id INT REFERENCES destinasi(destinasi_id) ON DELETE CASCADE,
                        tanggal_bookmark TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        UNIQUE(user_id, destinasi_id)
                    );

                    -- Insert sample data
                    INSERT INTO destinasi (nama_destinasi, lokasi, pulau, deskripsi, harga_min, harga_max, waktu_terbaik, rating_avg)
                    VALUES 
                    ('Raja Ampat Marine Park', 'Waisai, Raja Ampat, Papua Barat', 'Papua', 
                     'Surga bawah laut yang memiliki keindahan alam bawah laut terbaik di dunia', 
                     500000, 1000000, 'Oktober, November, Desember', 4.8)
                    ON CONFLICT DO NOTHING;
                ";

                using (var cmd = new NpgsqlCommand(createTables, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    // Class Entity - Base class untuk model
    public abstract class Entity
    {
        protected int id;
        protected DateTime createdAt;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }

        // Abstract method - akan di-override di child class
        public abstract bool Save();
        public abstract bool Delete();
    }

    // User Model - Inheritance
    public class User : Entity
    {
        private string namaLengkap;
        private string email;
        private string password;
        private string noTelepon;
        private string fotoProfil;

        // Encapsulation dengan Properties
        public string NamaLengkap
        {
            get { return namaLengkap; }
            set { namaLengkap = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Password { get; private set; }

        public void SetPassword(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw) || raw.Length < 6)
                throw new ArgumentException("Password harus minimal 6 karakter.");
            Password = HashPassword(raw);
        }

        public string NoTelepon
        {
            get { return noTelepon; }
            set { noTelepon = value; }
        }

        public string FotoProfil
        {
            get { return fotoProfil; }
            set { fotoProfil = value; }
        }

        // Static property untuk menyimpan user yang sedang login
        public static User CurrentUser { get; set; }

        public override bool Save()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = @"INSERT INTO users (nama_lengkap, email, password, no_telepon) 
                                    VALUES (@nama, @email, @password, @telepon) RETURNING user_id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", namaLengkap);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", HashPassword(password));
                        cmd.Parameters.AddWithValue("@telepon", noTelepon ?? "");

                        id = Convert.ToInt32(cmd.ExecuteScalar());
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error menyimpan user: {ex.Message}", "Error");
                return false;
            }
        }

        public override bool Delete()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = "DELETE FROM users WHERE user_id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error menghapus user: {ex.Message}", "Error");
                return false;
            }
        }

        public bool Update()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = @"UPDATE users 
                                    SET nama_lengkap = @nama, email = @email, 
                                        no_telepon = @telepon, foto_profil = @foto
                                    WHERE user_id = @id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", namaLengkap);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@telepon", noTelepon ?? "");
                        cmd.Parameters.AddWithValue("@foto", fotoProfil ?? "");
                        cmd.Parameters.AddWithValue("@id", id);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error update user: {ex.Message}", "Error");
                return false;
            }
        }

        public static User Login(string email, string password)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return null;

                    string query = @"SELECT user_id, nama_lengkap, email, no_telepon, 
                                           foto_profil, tanggal_bergabung 
                                    FROM users WHERE email = @email AND password = @password";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", HashPassword(password));

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var user = new User
                                {
                                    Id = reader.GetInt32(0),
                                    NamaLengkap = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    NoTelepon = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    FotoProfil = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    CreatedAt = reader.GetDateTime(5)
                                };
                                CurrentUser = user;
                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error login: {ex.Message}", "Error");
            }
            return null;
        }

        private static string HashPassword(string password)
        {
            // Simple hash - untuk production gunakan BCrypt atau SHA256
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    // Model Review - Inheritance
    public class Review : Entity
    {
        private int userId;
        private int destinasiId;
        private int rating;
        private string ulasan;
        private string namaPengguna;
        private string namaDestinasi;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int DestinasiId
        {
            get { return destinasiId; }
            set { destinasiId = value; }
        }

        public int Rating
        {
            get { return rating; }
            set { rating = Math.Max(1, Math.Min(5, value)); } // Validasi 1-5
        }

        public string Ulasan
        {
            get { return ulasan; }
            set { ulasan = value; }
        }

        public string NamaPengguna
        {
            get { return namaPengguna; }
            set { namaPengguna = value; }
        }

        public string NamaDestinasi
        {
            get { return namaDestinasi; }
            set { namaDestinasi = value; }
        }

        public override bool Save()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = @"INSERT INTO reviews (user_id, destinasi_id, rating, ulasan) 
                                    VALUES (@user_id, @destination_id, @rating, @ulasan) RETURNING review_id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        cmd.Parameters.AddWithValue("@destination_id", destinasiId);
                        cmd.Parameters.AddWithValue("@rating", rating);
                        cmd.Parameters.AddWithValue("@ulasan", ulasan ?? "");

                        id = Convert.ToInt32(cmd.ExecuteScalar());
                        UpdateDestinasiRating();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error menyimpan review: {ex.Message}", "Error");
                return false;
            }
        }

        public override bool Delete()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = "DELETE FROM reviews WHERE review_id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        bool result = cmd.ExecuteNonQuery() > 0;
                        if (result) UpdateDestinasiRating();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error menghapus review: {ex.Message}", "Error");
                return false;
            }
        }

        public bool Update()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = @"UPDATE reviews 
                                    SET rating = @rating, ulasan = @ulasan 
                                    WHERE review_id = @id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@rating", rating);
                        cmd.Parameters.AddWithValue("@ulasan", ulasan ?? "");
                        cmd.Parameters.AddWithValue("@id", id);

                        bool result = cmd.ExecuteNonQuery() > 0;
                        if (result) UpdateDestinasiRating();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error update review: {ex.Message}", "Error");
                return false;
            }
        }

        private void UpdateDestinasiRating()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return;

                    string query = @"UPDATE destinasi 
                                    SET rating_avg = (SELECT AVG(rating) FROM reviews WHERE destinasi_id = @destination_id),
                                        total_review = (SELECT COUNT(*) FROM reviews WHERE destinasi_id = @destination_id)
                                    WHERE destinasi_id = @destination_id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@destination_id", destinasiId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static List<Review> GetByUserId(int userId)
        {
            var reviews = new List<Review>();
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return reviews;

                    string query = @"SELECT r.review_id, r.user_id, r.destinasi_id, r.rating, 
                                           r.ulasan, r.tanggal_review, u.nama_lengkap, d.nama_destinasi
                                    FROM reviews r
                                    JOIN users u ON r.user_id = u.user_id
                                    JOIN destinasi d ON r.destinasi_id = d.destinasi_id
                                    WHERE r.user_id = @user_id
                                    ORDER BY r.tanggal_review DESC";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reviews.Add(new Review
                                {
                                    Id = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
                                    DestinasiId = reader.GetInt32(2),
                                    Rating = reader.GetInt32(3),
                                    Ulasan = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    CreatedAt = reader.GetDateTime(5),
                                    NamaPengguna = reader.GetString(6),
                                    NamaDestinasi = reader.GetString(7)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error mengambil reviews: {ex.Message}", "Error");
            }
            return reviews;
        }

        public static List<Review> GetByDestinasiId(int destinasiId)
        {
            var reviews = new List<Review>();
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return reviews;

                    string query = @"SELECT r.review_id, r.user_id, r.destinasi_id, r.rating, 
                                           r.ulasan, r.tanggal_review, u.nama_lengkap
                                    FROM reviews r
                                    JOIN users u ON r.user_id = u.user_id
                                    WHERE r.destinasi_id = @destination_id
                                    ORDER BY r.tanggal_review DESC";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@destination_id", destinasiId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reviews.Add(new Review
                                {
                                    Id = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
                                    DestinasiId = reader.GetInt32(2),
                                    Rating = reader.GetInt32(3),
                                    Ulasan = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    CreatedAt = reader.GetDateTime(5),
                                    NamaPengguna = reader.GetString(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error mengambil reviews destinasi: {ex.Message}", "Error");
            }
            return reviews;
        }
    }

    // Model Bookmark - Inheritance
    public class Bookmark : Entity
    {
        private int userId;
        private int destinasiId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int DestinasiId
        {
            get { return destinasiId; }
            set { destinasiId = value; }
        }

        public override bool Save()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = @"INSERT INTO bookmarks (user_id, destinasi_id) 
                                    VALUES (@user_id, @destination_id) 
                                    ON CONFLICT (user_id, destinasi_id) DO NOTHING 
                                    RETURNING bookmark_id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        cmd.Parameters.AddWithValue("@destination_id", destinasiId);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            id = Convert.ToInt32(result);
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error menyimpan bookmark: {ex.Message}", "Error");
                return false;
            }
        }

        public override bool Delete()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = "DELETE FROM bookmarks WHERE user_id = @user_id AND destinasi_id = @destination_id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        cmd.Parameters.AddWithValue("@destination_id", destinasiId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error menghapus bookmark: {ex.Message}", "Error");
                return false;
            }
        }

        public static bool IsBookmarked(int userId, int destinasiId)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return false;

                    string query = "SELECT COUNT(*) FROM bookmarks WHERE user_id = @user_id AND destinasi_id = @destination_id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        cmd.Parameters.AddWithValue("@destination_id", destinasiId);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static List<Destinasi> GetBookmarksByUserId(int userId)
        {
            var bookmarks = new List<Destinasi>();
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return bookmarks;

                    string query = @"SELECT d.* FROM destinasi d
                                    JOIN bookmarks b ON d.destinasi_id = b.destinasi_id
                                    WHERE b.user_id = @user_id
                                    ORDER BY b.tanggal_bookmark DESC";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bookmarks.Add(new Destinasi
                                {
                                    Id = reader.GetInt32(0),
                                    NamaDestinasi = reader.GetString(1),
                                    Lokasi = reader.GetString(2),
                                    Pulau = reader.GetString(3),
                                    Deskripsi = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    HargaMin = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                                    HargaMax = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                                    WaktuTerbaik = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                    GambarUrl = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                    RatingAvg = reader.IsDBNull(9) ? 0 : reader.GetDouble(9),
                                    TotalReview = reader.IsDBNull(10) ? 0 : reader.GetInt32(10)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error mengambil bookmarks: {ex.Message}", "Error");
            }
            return bookmarks;
        }
    }

    // Model Destinasi
    public class Destinasi
    {
        public int Id { get; set; }
        public string NamaDestinasi { get; set; }
        public string Lokasi { get; set; }
        public string Pulau { get; set; }
        public string Deskripsi { get; set; }
        public decimal HargaMin { get; set; }
        public decimal HargaMax { get; set; }
        public string WaktuTerbaik { get; set; }
        public string GambarUrl { get; set; }
        public double RatingAvg { get; set; }
        public int TotalReview { get; set; }

        public static Destinasi GetById(int id)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    if (conn == null) return null;

                    string query = "SELECT * FROM destinasi WHERE destinasi_id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Destinasi
                                {
                                    Id = reader.GetInt32(0),
                                    NamaDestinasi = reader.GetString(1),
                                    Lokasi = reader.GetString(2),
                                    Pulau = reader.GetString(3),
                                    Deskripsi = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    HargaMin = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                                    HargaMax = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                                    WaktuTerbaik = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                    GambarUrl = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                    RatingAvg = reader.IsDBNull(9) ? 0 : reader.GetDouble(9),
                                    TotalReview = reader.IsDBNull(10) ? 0 : reader.GetInt32(10)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error mengambil destinasi: {ex.Message}", "Error");
            }
            return null;
        }
    }
}