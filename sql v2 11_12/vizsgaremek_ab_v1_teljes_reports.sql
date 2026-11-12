-- =========================================================
-- vizsgaremek_ab_v1 - teljes adatbázis + reports (szerkezet + teszt adatok + barát-ellenőrzés trigger)
-- Futtatás: phpMyAdmin -> Importálás, vagy SQL fül -> beillesztés
-- A teszt felhasználók jelszava: Teszt123!   (bcrypt hash)
-- =========================================================

-- FIGYELEM: ha újra le akarod futtatni a teljes szkriptet a nulláról,
-- vedd ki a kommentet a következő sorból (ez TÖRLI a meglévő adatbázist!):
-- DROP DATABASE IF EXISTS `vizsgaremek_ab_v1`;

CREATE DATABASE IF NOT EXISTS `vizsgaremek_ab_v1`
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_general_ci;

USE `vizsgaremek_ab_v1`;
SET NAMES utf8mb4;
SET time_zone = "+00:00";

-- =========================================================
-- TÁBLÁK
-- =========================================================

-- ---------------------------------------------------------
-- users
-- ---------------------------------------------------------
CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `profile_pic` varchar(255) DEFAULT 'default_avatar.png',
  `bio` varchar(255) DEFAULT NULL,
  `is_admin` tinyint(1) NOT NULL DEFAULT 0,
  `is_banned` tinyint(1) NOT NULL DEFAULT 0,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------
-- posts (a content lehet NULL, így csak képes poszt is lehetséges)
-- ---------------------------------------------------------
CREATE TABLE `posts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `content` text DEFAULT NULL,
  `image_url` varchar(255) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `posts_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------
-- comments
-- ---------------------------------------------------------
CREATE TABLE `comments` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `post_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `content` text NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `post_id` (`post_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `comments_ibfk_1` FOREIGN KEY (`post_id`) REFERENCES `posts` (`id`) ON DELETE CASCADE,
  CONSTRAINT `comments_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------
-- post_likes (egy felhasználó egy posztot csak egyszer likeolhat)
-- ---------------------------------------------------------
CREATE TABLE `post_likes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `post_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_like` (`post_id`,`user_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `post_likes_ibfk_1` FOREIGN KEY (`post_id`) REFERENCES `posts` (`id`) ON DELETE CASCADE,
  CONSTRAINT `post_likes_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------
-- friendships (a fordított duplikációt az API ellenőrzi)
-- ---------------------------------------------------------
CREATE TABLE `friendships` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `requester_id` int(11) NOT NULL,
  `addressee_id` int(11) NOT NULL,
  `status` enum('pending','accepted','rejected') NOT NULL DEFAULT 'pending',
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_friendship` (`requester_id`,`addressee_id`),
  KEY `addressee_id` (`addressee_id`),
  CONSTRAINT `friendships_ibfk_1` FOREIGN KEY (`requester_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `friendships_ibfk_2` FOREIGN KEY (`addressee_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ---------------------------------------------------------
-- messages
-- ---------------------------------------------------------
CREATE TABLE `messages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sender_id` int(11) NOT NULL,
  `receiver_id` int(11) NOT NULL,
  `content` text NOT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `sender_id` (`sender_id`),
  KEY `receiver_id` (`receiver_id`),
  CONSTRAINT `messages_ibfk_1` FOREIGN KEY (`sender_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `messages_ibfk_2` FOREIGN KEY (`receiver_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- =========================================================
-- TESZT ADATOK
-- =========================================================

-- ---------------------------------------------------------
-- users
-- ---------------------------------------------------------
INSERT INTO `users` (`id`, `username`, `email`, `password_hash`, `profile_pic`, `bio`, `is_admin`, `is_banned`, `created_at`) VALUES
(1, 'kovacs_anna',  'anna.kovacs@example.com',  '$2b$12$tk9AdOF68ponrvDzCVsW8e3l9/6aNsbKmV2SSEqtCJd5FfbrvzkL2', 'default_avatar.png', 'Kávé, kód és túrák.',                   0, 0, '2026-09-01 08:15:00'),
(2, 'nagy_peter',   'peter.nagy@example.com',   '$2b$12$tk9AdOF68ponrvDzCVsW8e3l9/6aNsbKmV2SSEqtCJd5FfbrvzkL2', 'peter.jpg',          'Bükk-rajongó, hétvégén mindig úton.',  0, 0, '2026-09-02 10:30:00'),
(3, 'szabo_bence',  'bence.szabo@example.com',  '$2b$12$tk9AdOF68ponrvDzCVsW8e3l9/6aNsbKmV2SSEqtCJd5FfbrvzkL2', 'bence.jpg',          'Gamer és gépépítő.',                   0, 0, '2026-09-03 14:05:00'),
(4, 'toth_eszter',  'eszter.toth@example.com',  '$2b$12$tk9AdOF68ponrvDzCVsW8e3l9/6aNsbKmV2SSEqtCJd5FfbrvzkL2', 'eszter.png',         NULL,                                   0, 0, '2026-09-05 09:45:00'),
(5, 'horvath_mate', 'mate.horvath@example.com', '$2b$12$tk9AdOF68ponrvDzCVsW8e3l9/6aNsbKmV2SSEqtCJd5FfbrvzkL2', 'default_avatar.png', NULL,                                   0, 0, '2026-09-07 17:20:00'),
(6, 'varga_dora',   'dora.varga@example.com',   '$2b$12$tk9AdOF68ponrvDzCVsW8e3l9/6aNsbKmV2SSEqtCJd5FfbrvzkL2', 'dora.jpg',           NULL,                                   0, 0, '2026-09-10 12:00:00'),
(7, 'admin',        'admin@example.com',        '$2b$12$tk9AdOF68ponrvDzCVsW8e3l9/6aNsbKmV2SSEqtCJd5FfbrvzkL2', 'default_avatar.png', 'Az oldal adminisztrátora.',           1, 0, '2026-08-30 09:00:00');

-- Ha ki akarod próbálni a tiltást a WPF-ben, vedd ki a kommentet:
-- UPDATE `users` SET `is_banned` = 1 WHERE `id` = 5;

-- ---------------------------------------------------------
-- friendships (requester -> addressee)
-- ---------------------------------------------------------
INSERT INTO `friendships` (`id`, `requester_id`, `addressee_id`, `status`, `created_at`) VALUES
(1, 1, 2, 'accepted', '2026-09-04 11:00:00'),
(2, 1, 3, 'accepted', '2026-09-04 11:05:00'),
(3, 2, 3, 'accepted', '2026-09-06 15:30:00'),
(4, 4, 1, 'pending',  '2026-10-20 18:40:00'),
(5, 5, 2, 'rejected', '2026-09-15 09:10:00'),
(6, 6, 4, 'accepted', '2026-09-18 20:00:00'),
(7, 3, 6, 'pending',  '2026-10-25 13:25:00');

-- ---------------------------------------------------------
-- posts
-- ---------------------------------------------------------
INSERT INTO `posts` (`id`, `user_id`, `content`, `image_url`, `created_at`) VALUES
(1, 1, 'Első posztom az új oldalon! Sziasztok mindenkinek! 👋',      NULL,                         '2026-09-04 12:00:00'),
(2, 2, 'Ma a Bükkben túráztunk, csodás idő volt.',                  'uploads/bukk_tura.jpg',      '2026-09-12 16:45:00'),
(3, 3, 'Új gépet építettem, végre nem akad a Visual Studio 😄',      'uploads/pc_build.jpg',       '2026-09-20 19:10:00'),
(4, 1, 'Kávé és kód, jó hétvégét!',                                 'uploads/kave.jpg',           '2026-10-01 08:30:00'),
(5, 4, 'Valaki ismer jó helyet Miskolcon, ahol lehet tanulni?',     NULL,                         '2026-10-05 10:15:00'),
(6, 6, 'Naplemente a Diósgyőri vár mellett.',                       'uploads/var_naplemente.jpg', '2026-10-14 18:50:00'),
(7, 2, 'Vizsgaremek határidő közeleg... ki van még lemaradva? 😅',  NULL,                         '2026-10-28 21:00:00'),
(8, 5, 'Kipróbáltam az új étteremet a belvárosban, ajánlom!',       'uploads/etterem.jpg',        '2026-11-01 13:20:00'),
(9, 3, NULL,                                                        'uploads/csak_kep.jpg',       '2026-11-02 10:00:00');

-- ---------------------------------------------------------
-- comments
-- ---------------------------------------------------------
INSERT INTO `comments` (`id`, `post_id`, `user_id`, `content`, `created_at`) VALUES
(1,  1, 2, 'Üdv az oldalon, Anna!',                    '2026-09-04 12:30:00'),
(2,  1, 3, 'Szia! Jó, hogy itt vagy.',                 '2026-09-04 13:10:00'),
(3,  2, 1, 'Gyönyörű kép, hol van ez pontosan?',       '2026-09-12 17:20:00'),
(4,  2, 3, 'Legközelebb én is jövök!',                 '2026-09-12 18:00:00'),
(5,  3, 2, 'Milyen videókártyát raktál bele?',         '2026-09-20 19:40:00'),
(6,  3, 1, 'Gratulálok, szép lett!',                   '2026-09-21 08:05:00'),
(7,  5, 6, 'A városi könyvtár szerintem tökéletes.',   '2026-10-05 11:00:00'),
(8,  6, 4, 'Imádom ezt a helyet! 😍',                  '2026-10-14 19:30:00'),
(9,  7, 1, 'Mi még dolgozunk rajta, de haladunk.',     '2026-10-28 21:30:00'),
(10, 7, 3, 'Én még az adatbázisnál tartok :D',         '2026-10-28 22:05:00');

-- ---------------------------------------------------------
-- post_likes
-- ---------------------------------------------------------
INSERT INTO `post_likes` (`id`, `post_id`, `user_id`, `created_at`) VALUES
(1,  1, 2, '2026-09-04 12:25:00'),
(2,  1, 3, '2026-09-04 13:00:00'),
(3,  2, 1, '2026-09-12 17:15:00'),
(4,  2, 3, '2026-09-12 17:50:00'),
(5,  2, 6, '2026-09-13 09:00:00'),
(6,  3, 1, '2026-09-21 08:00:00'),
(7,  3, 2, '2026-09-20 19:35:00'),
(8,  4, 2, '2026-10-01 09:00:00'),
(9,  4, 3, '2026-10-01 10:30:00'),
(10, 5, 1, '2026-10-05 10:45:00'),
(11, 6, 4, '2026-10-14 19:25:00'),
(12, 6, 1, '2026-10-15 07:40:00'),
(13, 7, 1, '2026-10-28 21:20:00'),
(14, 7, 3, '2026-10-28 22:00:00'),
(15, 8, 2, '2026-11-01 14:00:00');

-- ---------------------------------------------------------
-- messages
-- ---------------------------------------------------------
INSERT INTO `messages` (`id`, `sender_id`, `receiver_id`, `content`, `created_at`) VALUES
(1,  1, 2, 'Szia Peti! Mizu?',                                         '2026-09-05 18:00:00'),
(2,  2, 1, 'Szia Anna! Semmi extra, te?',                              '2026-09-05 18:02:00'),
(3,  1, 2, 'Én is csak a házit csinálom. Holnap találkozunk suliban?', '2026-09-05 18:05:00'),
(4,  2, 1, 'Persze, ott leszek!',                                      '2026-09-05 18:06:00'),
(5,  3, 1, 'Anna, megvan a projekt adatbázisterve?',                   '2026-10-10 15:30:00'),
(6,  1, 3, 'Igen, este átküldöm.',                                     '2026-10-10 15:45:00'),
(7,  3, 2, 'Peti, jössz ma edzésre?',                                  '2026-10-12 16:00:00'),
(8,  2, 3, 'Nem tudok, tanulnom kell.',                                '2026-10-12 16:10:00'),
(9,  6, 4, 'Eszter, ráérsz hétvégén?',                                 '2026-10-20 20:00:00'),
(10, 4, 6, 'Szombaton igen, mit terveztél?',                           '2026-10-20 20:10:00'),
(11, 6, 4, 'Moziba mennék, jössz?',                                    '2026-10-20 20:12:00');

-- =========================================================
-- REPORTOLÁS (jelentés) KIEGÉSZÍTÉS
-- =========================================================

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
-- TRIGGER: üzenetet csak elfogadott (accepted) barátok küldhetnek egymásnak
-- (a barátság iránya mindegy: requester->addressee vagy fordítva)
-- Az adatok betöltése UTÁN jön létre, hogy ne zavarja a teszt adatok beszúrását.
-- Hiba esetén SQLSTATE 45000 -> az API ezt elkapja és 403-at adhat vissza.
-- =========================================================
DROP TRIGGER IF EXISTS `trg_messages_friends_only`;

DELIMITER $$
CREATE TRIGGER `trg_messages_friends_only`
BEFORE INSERT ON `messages`
FOR EACH ROW
BEGIN
  IF NEW.sender_id = NEW.receiver_id THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Saját magadnak nem küldhetsz üzenetet.';
  END IF;

  IF NOT EXISTS (
    SELECT 1 FROM `friendships`
    WHERE `status` = 'accepted'
      AND ((`requester_id` = NEW.sender_id   AND `addressee_id` = NEW.receiver_id)
        OR (`requester_id` = NEW.receiver_id AND `addressee_id` = NEW.sender_id))
  ) THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Üzenetet csak barátoknak küldhetsz.';
  END IF;
END$$
DELIMITER ;

-- Kipróbálás (hibát kell dobnia, mert 1 és 6 nem barátok):
-- INSERT INTO `messages` (`sender_id`, `receiver_id`, `content`) VALUES (1, 6, 'Szia!');

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
