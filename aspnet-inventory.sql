/*
 Navicat Premium Data Transfer

 Source Server         : ilab-db
 Source Server Type    : MySQL
 Source Server Version : 110302 (11.3.2-MariaDB-1:11.3.2+maria~ubu2204)
 Source Host           : localhost:3306
 Source Schema         : aspnet-inventory

 Target Server Type    : MySQL
 Target Server Version : 110302 (11.3.2-MariaDB-1:11.3.2+maria~ubu2204)
 File Encoding         : 65001

 Date: 18/03/2024 13:53:39
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for Items
-- ----------------------------
DROP TABLE IF EXISTS `Items`;
CREATE TABLE `Items`  (
  `InventoryID` int NOT NULL AUTO_INCREMENT,
  `ItemName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ItemDescription` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ItemQuantity` int NOT NULL,
  `ItemPrice` decimal(65, 30) NOT NULL,
  `Category` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`InventoryID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Items
-- ----------------------------

-- ----------------------------
-- Table structure for Users
-- ----------------------------
DROP TABLE IF EXISTS `Users`;
CREATE TABLE `Users`  (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdateAt` datetime(6) NOT NULL,
  PRIMARY KEY (`UserID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Users
-- ----------------------------
INSERT INTO `Users` VALUES (1, 'rizky', 'rizky123', 'rizky@example.com', '2024-03-18 06:44:58.538000', '2024-03-18 06:44:58.538000');

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory`  (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of __EFMigrationsHistory
-- ----------------------------
INSERT INTO `__EFMigrationsHistory` VALUES ('20240318063103_InitialCreate', '8.0.3');

SET FOREIGN_KEY_CHECKS = 1;
