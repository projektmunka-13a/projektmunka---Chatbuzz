-- =========================================================
-- Reportolás (jelentés) funkció - kiegészítés a vizsgaremek_ab_v1-hez
-- Ha a vizsgaremek_ab_v1_teljes.sql már tartalmazza, ezt NEM kell külön futtatni.
-- =========================================================

USE `vizsgaremek_ab_v1`;
SET NAMES utf8mb4;

-- ---------------------------------------------------------
-- reports
-- Egy jelentés pontosan EGY dologra vonatkozik:
--   post_id  VAGY  comment_id  VAGY  reported_user_id
-- ---------------------------------------------------------
CREATE TABLE `reports` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `reporter_id` int(11) NOT NULL,
  `post_id` int(11) DEFAULT NULL,
  `comment_id` int(11) DEFAULT NULL,
  `reported_user_id` int(11) DEFAULT NULL,
  `reason` enum('spam','harassment','hate_speech','nudity','violence','other') NOT NULL DEFAULT 'other',
  `description` varchar(500) DEFAULT NULL,
  `status` enum('pending','reviewed','dismissed') NOT NULL DEFAULT 'pending',
  `reviewed_by` int(11) DEFAULT NULL,
  `reviewed_at` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_report_post` (`reporter_id`,`post_id`),
  UNIQUE KEY `unique_report_comment` (`reporter_id`,`comment_id`),
  UNIQUE KEY `unique_report_user` (`reporter_id`,`reported_user_id`),
  KEY `post_id` (`post_id`),
  KEY `comment_id` (`comment_id`),
  KEY `reported_user_id` (`reported_user_id`),
  KEY `reviewed_by` (`reviewed_by`),
  KEY `idx_reports_status_created` (`status`,`created_at`),
  CONSTRAINT `reports_ibfk_1` FOREIGN KEY (`reporter_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `reports_ibfk_2` FOREIGN KEY (`post_id`) REFERENCES `posts` (`id`) ON DELETE CASCADE,
  CONSTRAINT `reports_ibfk_3` FOREIGN KEY (`comment_id`) REFERENCES `comments` (`id`) ON DELETE CASCADE,
  CONSTRAINT `reports_ibfk_4` FOREIGN KEY (`reported_user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `reports_ibfk_5` FOREIGN KEY (`reviewed_by`) REFERENCES `users` (`id`) ON DELETE SET NULL,
  CONSTRAINT `chk_report_one_target` CHECK ((`post_id` IS NOT NULL) + (`comment_id` IS NOT NULL) + (`reported_user_id` IS NOT NULL) = 1),
  CONSTRAINT `chk_report_not_self` CHECK (`reported_user_id` IS NULL OR `reporter_id` <> `reported_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------
-- Teszt adatok
-- ---------------------------------------------------------
INSERT INTO `reports` (`id`, `reporter_id`, `post_id`, `comment_id`, `reported_user_id`, `reason`, `description`, `status`, `reviewed_by`, `reviewed_at`, `created_at`) VALUES
(1, 3, 8,    NULL, NULL, 'spam',        'Reklámnak tűnik, többször posztolja.',        'pending',   NULL, NULL,                  '2026-11-02 09:15:00'),
(2, 1, NULL, 10,   NULL, 'harassment',  'Sértő hangnem a kommentek között.',           'pending',   NULL, NULL,                  '2026-11-02 11:40:00'),
(3, 6, NULL, NULL, 5,    'spam',        'Folyamatosan spam üzeneteket küld.',          'pending',   NULL, NULL,                  '2026-11-03 08:20:00'),
(4, 2, 5,    NULL, NULL, 'other',       NULL,                                          'dismissed', 7,    '2026-11-03 10:00:00', '2026-11-02 18:05:00'),
(5, 4, NULL, 6,    NULL, 'hate_speech', 'Nem megfelelő megfogalmazás.',                'reviewed',  7,    '2026-11-03 10:30:00', '2026-11-02 19:30:00');

-- =========================================================
-- ADMIN LEKÉRDEZÉSEK (példák az API-hoz / WPF-hez)
-- =========================================================

-- 1) Jelentések listája (a függőben lévők elöl)
-- SELECT r.id, r.status, r.reason, r.description, r.created_at,
--        reporter.username AS reporter,
--        CASE WHEN r.post_id IS NOT NULL THEN 'post'
--             WHEN r.comment_id IS NOT NULL THEN 'comment'
--             ELSE 'user' END AS target_type
-- FROM reports r
-- JOIN users reporter ON reporter.id = r.reporter_id
-- ORDER BY (r.status = 'pending') DESC, r.created_at DESC;

-- 2) Komment-jelentésnél: maga a poszt (a komment azonosítójából)
-- SELECT p.*, u.username
-- FROM comments c
-- JOIN posts p ON p.id = c.post_id
-- JOIN users u ON u.id = p.user_id
-- WHERE c.id = 10;

-- 3) Komment-jelentésnél: a poszt ÖSSZES kommentje (a jelentettet az id alapján lehet kiemelni)
-- SELECT c.id, c.content, c.created_at, u.username
-- FROM comments c
-- JOIN users u ON u.id = c.user_id
-- WHERE c.post_id = (SELECT post_id FROM comments WHERE id = 10)
-- ORDER BY c.created_at;
