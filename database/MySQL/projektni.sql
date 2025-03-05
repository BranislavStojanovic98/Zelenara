-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: projektni
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `dobavljac`
--

DROP TABLE IF EXISTS `dobavljac`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dobavljac` (
  `idDostavljaca` int NOT NULL,
  `Naziv` varchar(45) NOT NULL,
  `Adresa` varchar(45) NOT NULL,
  `MJESTO_PostanskiBroj` int NOT NULL,
  PRIMARY KEY (`idDostavljaca`),
  KEY `fk_DOSTAVLJAC_MJESTO1_idx` (`MJESTO_PostanskiBroj`),
  CONSTRAINT `fk_DOSTAVLJAC_MJESTO1` FOREIGN KEY (`MJESTO_PostanskiBroj`) REFERENCES `mjesto` (`PostanskiBroj`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dobavljac`
--

LOCK TABLES `dobavljac` WRITE;
/*!40000 ALTER TABLE `dobavljac` DISABLE KEYS */;
INSERT INTO `dobavljac` VALUES (1,'Dobavljac1','Neko Nesto',1),(2,'Dobavljac2','Dobavljacka bb',1);
/*!40000 ALTER TABLE `dobavljac` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `isporuka`
--

DROP TABLE IF EXISTS `isporuka`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `isporuka` (
  `idIsporuke` int NOT NULL,
  `NABAVKA_idPotvrde` int NOT NULL,
  `Datum` date NOT NULL,
  `DOBAVLJAC_idDostavljaca` int NOT NULL,
  PRIMARY KEY (`idIsporuke`,`NABAVKA_idPotvrde`),
  KEY `fk_ISPORUKA_NABAVKA1_idx` (`NABAVKA_idPotvrde`),
  KEY `fk_DOSTAVLJAC_has_RADNJA_DOSTAVLJAC1_idx` (`DOBAVLJAC_idDostavljaca`),
  CONSTRAINT `fk_DOSTAVLJAC_has_RADNJA_DOSTAVLJAC1` FOREIGN KEY (`DOBAVLJAC_idDostavljaca`) REFERENCES `dobavljac` (`idDostavljaca`),
  CONSTRAINT `fk_ISPORUKA_NABAVKA1` FOREIGN KEY (`NABAVKA_idPotvrde`) REFERENCES `nabavka` (`idPotvrde`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `isporuka`
--

LOCK TABLES `isporuka` WRITE;
/*!40000 ALTER TABLE `isporuka` DISABLE KEYS */;
INSERT INTO `isporuka` VALUES (1,1,'2025-03-02',1),(2,1,'2025-03-02',1),(4,2,'2025-03-02',2),(5,2,'2025-03-02',2),(6,3,'2025-03-02',1),(7,3,'2025-03-02',1),(10,2,'2025-03-02',2),(11,3,'2025-03-05',1),(12,3,'2025-03-05',1),(13,3,'2025-03-05',1),(14,3,'2025-03-05',1),(15,3,'2025-03-05',1),(16,3,'2025-03-05',1),(17,3,'2025-03-05',1),(18,3,'2025-03-05',1),(19,3,'2025-03-05',1),(20,3,'2025-03-05',1),(21,3,'2025-03-05',1),(22,3,'2025-03-05',1);
/*!40000 ALTER TABLE `isporuka` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `isporuka_dobavljaca`
--

DROP TABLE IF EXISTS `isporuka_dobavljaca`;
/*!50001 DROP VIEW IF EXISTS `isporuka_dobavljaca`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `isporuka_dobavljaca` AS SELECT 
 1 AS `ID_Dostavljaca`,
 1 AS `Naziv`,
 1 AS `Adresa`,
 1 AS `ID_Isporuke`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `isporuka_produkta`
--

DROP TABLE IF EXISTS `isporuka_produkta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `isporuka_produkta` (
  `DOSTAVA_idIsporuke` int NOT NULL,
  `PRODUKT_idProdukta` int NOT NULL,
  `Kolicina` int NOT NULL,
  PRIMARY KEY (`DOSTAVA_idIsporuke`,`PRODUKT_idProdukta`),
  KEY `fk_PRODUKT_has_DOSTAVA_DOSTAVA1_idx` (`DOSTAVA_idIsporuke`),
  KEY `fk_ISPORUKA_PRODUKTA_PRODUKT1_idx` (`PRODUKT_idProdukta`),
  CONSTRAINT `fk_ISPORUKA_PRODUKTA_PRODUKT1` FOREIGN KEY (`PRODUKT_idProdukta`) REFERENCES `produkt` (`idProdukta`),
  CONSTRAINT `fk_PRODUKT_has_DOSTAVA_DOSTAVA1` FOREIGN KEY (`DOSTAVA_idIsporuke`) REFERENCES `isporuka` (`idIsporuke`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `isporuka_produkta`
--

LOCK TABLES `isporuka_produkta` WRITE;
/*!40000 ALTER TABLE `isporuka_produkta` DISABLE KEYS */;
INSERT INTO `isporuka_produkta` VALUES (1,1,1),(2,2,23),(4,3,11),(5,1,1),(6,1,10),(7,2,10),(10,1,1),(18,1,3),(19,1,2),(20,1,1),(21,1,1),(22,1,1);
/*!40000 ALTER TABLE `isporuka_produkta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `isporuke_dostavljaca`
--

DROP TABLE IF EXISTS `isporuke_dostavljaca`;
/*!50001 DROP VIEW IF EXISTS `isporuke_dostavljaca`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `isporuke_dostavljaca` AS SELECT 
 1 AS `ID_produkta`,
 1 AS `Naziv`,
 1 AS `Vrsta`,
 1 AS `Dostavljena_kolicina`,
 1 AS `Proizvodjac`,
 1 AS `Datum_isporuke`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `isporuke_lista`
--

DROP TABLE IF EXISTS `isporuke_lista`;
/*!50001 DROP VIEW IF EXISTS `isporuke_lista`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `isporuke_lista` AS SELECT 
 1 AS `ID_isporuke`,
 1 AS `ID_nabavke`,
 1 AS `isplistUkupno`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `isporuke_nabavke`
--

DROP TABLE IF EXISTS `isporuke_nabavke`;
/*!50001 DROP VIEW IF EXISTS `isporuke_nabavke`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `isporuke_nabavke` AS SELECT 
 1 AS `ID_nabavke`,
 1 AS `Dostavljac`,
 1 AS `Ukupna_cena`,
 1 AS `datum_posl_isp`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `kupac`
--

DROP TABLE IF EXISTS `kupac`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kupac` (
  `RAČUN_idRacuna` int NOT NULL,
  KEY `fk_KUPAC_RAČUN1_idx` (`RAČUN_idRacuna`),
  CONSTRAINT `fk_KUPAC_RAČUN1` FOREIGN KEY (`RAČUN_idRacuna`) REFERENCES `račun` (`idRacuna`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kupac`
--

LOCK TABLES `kupac` WRITE;
/*!40000 ALTER TABLE `kupac` DISABLE KEYS */;
/*!40000 ALTER TABLE `kupac` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kupovina_produkta`
--

DROP TABLE IF EXISTS `kupovina_produkta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kupovina_produkta` (
  `RAČUN_idRacuna` int NOT NULL,
  `Kolicina` int NOT NULL,
  `PRODUKT_idProdukta` int NOT NULL,
  PRIMARY KEY (`RAČUN_idRacuna`),
  KEY `fk_INSTANCA_PRODUKTA_has_RAČUN_RAČUN1_idx` (`RAČUN_idRacuna`),
  KEY `fk_KUPOVINA_PRODUKTA_PRODUKT1_idx` (`PRODUKT_idProdukta`),
  CONSTRAINT `fk_INSTANCA_PRODUKTA_has_RAČUN_RAČUN1` FOREIGN KEY (`RAČUN_idRacuna`) REFERENCES `račun` (`idRacuna`),
  CONSTRAINT `fk_KUPOVINA_PRODUKTA_PRODUKT1` FOREIGN KEY (`PRODUKT_idProdukta`) REFERENCES `produkt` (`idProdukta`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kupovina_produkta`
--

LOCK TABLES `kupovina_produkta` WRITE;
/*!40000 ALTER TABLE `kupovina_produkta` DISABLE KEYS */;
/*!40000 ALTER TABLE `kupovina_produkta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menadzer`
--

DROP TABLE IF EXISTS `menadzer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menadzer` (
  `ZAPOSLENI_JMB` char(12) NOT NULL,
  PRIMARY KEY (`ZAPOSLENI_JMB`),
  CONSTRAINT `fk_MENADZER_ZAPOSLENI1` FOREIGN KEY (`ZAPOSLENI_JMB`) REFERENCES `zaposleni` (`JMB`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menadzer`
--

LOCK TABLES `menadzer` WRITE;
/*!40000 ALTER TABLE `menadzer` DISABLE KEYS */;
INSERT INTO `menadzer` VALUES ('111111111111');
/*!40000 ALTER TABLE `menadzer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mjesto`
--

DROP TABLE IF EXISTS `mjesto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mjesto` (
  `PostanskiBroj` int NOT NULL,
  `Naziv` varchar(45) NOT NULL,
  PRIMARY KEY (`PostanskiBroj`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mjesto`
--

LOCK TABLES `mjesto` WRITE;
/*!40000 ALTER TABLE `mjesto` DISABLE KEYS */;
INSERT INTO `mjesto` VALUES (1,'BL'),(2,'BR'),(3,'TR'),(4,'KR'),(5,'BG'),(6,'TZ'),(7,'LK');
/*!40000 ALTER TABLE `mjesto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `mjesto_sa_postanskimbr`
--

DROP TABLE IF EXISTS `mjesto_sa_postanskimbr`;
/*!50001 DROP VIEW IF EXISTS `mjesto_sa_postanskimbr`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `mjesto_sa_postanskimbr` AS SELECT 
 1 AS `infoMjesta`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `nabavka`
--

DROP TABLE IF EXISTS `nabavka`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nabavka` (
  `idPotvrde` int NOT NULL,
  `SKLADISTE_idSkladista` int NOT NULL,
  `Datum` date NOT NULL,
  `MENADZER_ZAPOSLENI_JMB` char(12) NOT NULL,
  `DOBAVLJAC_idDostavljaca` int NOT NULL,
  PRIMARY KEY (`idPotvrde`),
  KEY `fk_NABAVKA_MENADZER1_idx` (`MENADZER_ZAPOSLENI_JMB`),
  KEY `fk_NABAVKA_DOBAVLJAC1_idx` (`DOBAVLJAC_idDostavljaca`),
  KEY `fk_NABAVKA_SKLADISTE1_idx` (`SKLADISTE_idSkladista`),
  CONSTRAINT `fk_NABAVKA_DOBAVLJAC1` FOREIGN KEY (`DOBAVLJAC_idDostavljaca`) REFERENCES `dobavljac` (`idDostavljaca`),
  CONSTRAINT `fk_NABAVKA_MENADZER1` FOREIGN KEY (`MENADZER_ZAPOSLENI_JMB`) REFERENCES `menadzer` (`ZAPOSLENI_JMB`),
  CONSTRAINT `fk_NABAVKA_SKLADISTE1` FOREIGN KEY (`SKLADISTE_idSkladista`) REFERENCES `skladiste` (`idSkladista`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nabavka`
--

LOCK TABLES `nabavka` WRITE;
/*!40000 ALTER TABLE `nabavka` DISABLE KEYS */;
INSERT INTO `nabavka` VALUES (1,1,'2025-03-02','111111111111',1),(2,1,'2025-03-02','111111111111',2),(3,1,'2025-03-02','111111111111',1);
/*!40000 ALTER TABLE `nabavka` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nabavka_produkta`
--

DROP TABLE IF EXISTS `nabavka_produkta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nabavka_produkta` (
  `Kolicina` int NOT NULL,
  `Cena` decimal(10,2) NOT NULL,
  `NABAVKA_idPotvrde` int NOT NULL,
  `isporuka_produkta_DOSTAVA_idIsporuke` int NOT NULL,
  `isporuka_produkta_PRODUKT_idProdukta` int NOT NULL,
  PRIMARY KEY (`NABAVKA_idPotvrde`,`isporuka_produkta_DOSTAVA_idIsporuke`,`isporuka_produkta_PRODUKT_idProdukta`),
  KEY `fk_NABAVKA_PRODUKTA_NABAVKA1_idx` (`NABAVKA_idPotvrde`),
  KEY `fk_nabavka_produkta_isporuka_produkta1_idx` (`isporuka_produkta_DOSTAVA_idIsporuke`,`isporuka_produkta_PRODUKT_idProdukta`),
  CONSTRAINT `fk_nabavka_produkta_isporuka_produkta1` FOREIGN KEY (`isporuka_produkta_DOSTAVA_idIsporuke`, `isporuka_produkta_PRODUKT_idProdukta`) REFERENCES `isporuka_produkta` (`DOSTAVA_idIsporuke`, `PRODUKT_idProdukta`),
  CONSTRAINT `fk_NABAVKA_PRODUKTA_NABAVKA1` FOREIGN KEY (`NABAVKA_idPotvrde`) REFERENCES `nabavka` (`idPotvrde`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nabavka_produkta`
--

LOCK TABLES `nabavka_produkta` WRITE;
/*!40000 ALTER TABLE `nabavka_produkta` DISABLE KEYS */;
INSERT INTO `nabavka_produkta` VALUES (1,4.26,1,1,1),(23,28.75,1,2,2),(11,12.54,2,4,3),(1,4.26,2,5,1),(1,4.26,2,10,1),(10,42.60,3,6,1),(10,12.50,3,7,2),(3,12.78,3,18,1),(2,8.52,3,19,1),(1,4.26,3,20,1),(1,4.26,3,21,1),(1,4.26,3,22,1);
/*!40000 ALTER TABLE `nabavka_produkta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `pregled_dostavljaca`
--

DROP TABLE IF EXISTS `pregled_dostavljaca`;
/*!50001 DROP VIEW IF EXISTS `pregled_dostavljaca`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `pregled_dostavljaca` AS SELECT 
 1 AS `ID_Dostavljaca`,
 1 AS `Naziv`,
 1 AS `Adresa`,
 1 AS `Mjesto`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `pregled_isporuka`
--

DROP TABLE IF EXISTS `pregled_isporuka`;
/*!50001 DROP VIEW IF EXISTS `pregled_isporuka`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `pregled_isporuka` AS SELECT 
 1 AS `Dostavljac`,
 1 AS `ID_nabavke`,
 1 AS `ID_produkta`,
 1 AS `Naziv`,
 1 AS `Vrsta`,
 1 AS `Dostavljena_kolicina`,
 1 AS `Proizvodjac`,
 1 AS `Datum_isporuke`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `pregled_isporuka_nabake`
--

DROP TABLE IF EXISTS `pregled_isporuka_nabake`;
/*!50001 DROP VIEW IF EXISTS `pregled_isporuka_nabake`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `pregled_isporuka_nabake` AS SELECT 
 1 AS `IDProdukta`,
 1 AS `Produkt`,
 1 AS `Vrsta`,
 1 AS `Kolicina`,
 1 AS `Proizvodjac`,
 1 AS `Cena`,
 1 AS `Datum_isporuke`,
 1 AS `IDNabavke`,
 1 AS `IDIsporuke`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `pregled_produkta`
--

DROP TABLE IF EXISTS `pregled_produkta`;
/*!50001 DROP VIEW IF EXISTS `pregled_produkta`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `pregled_produkta` AS SELECT 
 1 AS `ID produkta`,
 1 AS `produktInfo`,
 1 AS `Proizvodjac`,
 1 AS `Cena`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `pregled_skladista`
--

DROP TABLE IF EXISTS `pregled_skladista`;
/*!50001 DROP VIEW IF EXISTS `pregled_skladista`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `pregled_skladista` AS SELECT 
 1 AS `ID_produkta`,
 1 AS `Naziv_produkta`,
 1 AS `Vrsta_produkta`,
 1 AS `Kolicina`,
 1 AS `Cena`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `pregled_zaposlenih`
--

DROP TABLE IF EXISTS `pregled_zaposlenih`;
/*!50001 DROP VIEW IF EXISTS `pregled_zaposlenih`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `pregled_zaposlenih` AS SELECT 
 1 AS `JMB`,
 1 AS `Ime`,
 1 AS `Prezime`,
 1 AS `Mjesto_Stanovanja`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `produkt`
--

DROP TABLE IF EXISTS `produkt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `produkt` (
  `idProdukta` int NOT NULL,
  `Naziv` varchar(45) NOT NULL,
  `Vrsta` varchar(45) NOT NULL,
  `Proizvodjac` varchar(45) NOT NULL,
  `Cena` decimal(10,2) NOT NULL,
  PRIMARY KEY (`idProdukta`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `produkt`
--

LOCK TABLES `produkt` WRITE;
/*!40000 ALTER TABLE `produkt` DISABLE KEYS */;
INSERT INTO `produkt` VALUES (1,'Paradajz','Crveni','Paramax',4.26),(2,'Jabuke','Crvene','Jabumax',1.25),(3,'Jabuke','Zelene','Jabumax',1.14);
/*!40000 ALTER TABLE `produkt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `promocija`
--

DROP TABLE IF EXISTS `promocija`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `promocija` (
  `idPromocije` int NOT NULL,
  `odDatumPromocije` date NOT NULL,
  `doDatumPromocije` date NOT NULL,
  PRIMARY KEY (`idPromocije`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `promocija`
--

LOCK TABLES `promocija` WRITE;
/*!40000 ALTER TABLE `promocija` DISABLE KEYS */;
/*!40000 ALTER TABLE `promocija` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `promocija_instance_produkta`
--

DROP TABLE IF EXISTS `promocija_instance_produkta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `promocija_instance_produkta` (
  `Popust` float NOT NULL,
  `PROMOCIJA_idPromocije` int NOT NULL,
  `PRODUKT_idProdukta` int NOT NULL,
  KEY `fk_PROMOCIJA_INSTANCE_PRODUKTA_PRODUKT1_idx` (`PRODUKT_idProdukta`),
  KEY `fk_PROMOCIJA_INSTANCE_PRODUKTA_PROMOCIJA1` (`PROMOCIJA_idPromocije`),
  CONSTRAINT `fk_PROMOCIJA_INSTANCE_PRODUKTA_PRODUKT1` FOREIGN KEY (`PRODUKT_idProdukta`) REFERENCES `produkt` (`idProdukta`),
  CONSTRAINT `fk_PROMOCIJA_INSTANCE_PRODUKTA_PROMOCIJA1` FOREIGN KEY (`PROMOCIJA_idPromocije`) REFERENCES `promocija` (`idPromocije`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `promocija_instance_produkta`
--

LOCK TABLES `promocija_instance_produkta` WRITE;
/*!40000 ALTER TABLE `promocija_instance_produkta` DISABLE KEYS */;
/*!40000 ALTER TABLE `promocija_instance_produkta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `radnik`
--

DROP TABLE IF EXISTS `radnik`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `radnik` (
  `ZAPOSLENI_JMB` char(12) NOT NULL,
  PRIMARY KEY (`ZAPOSLENI_JMB`),
  CONSTRAINT `fk_RADNIK_ZAPOSLENI1` FOREIGN KEY (`ZAPOSLENI_JMB`) REFERENCES `zaposleni` (`JMB`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `radnik`
--

LOCK TABLES `radnik` WRITE;
/*!40000 ALTER TABLE `radnik` DISABLE KEYS */;
/*!40000 ALTER TABLE `radnik` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `račun`
--

DROP TABLE IF EXISTS `račun`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `račun` (
  `idRacuna` int NOT NULL AUTO_INCREMENT,
  `VremeIzdavanja` datetime NOT NULL,
  `Kupljeni_Produkti` varchar(455) NOT NULL,
  `Cena` decimal(10,2) NOT NULL,
  `RADNIK_ZAPOSLENI_JMB` char(12) NOT NULL,
  PRIMARY KEY (`idRacuna`),
  KEY `fk_RAČUN_RADNIK1_idx` (`RADNIK_ZAPOSLENI_JMB`),
  CONSTRAINT `fk_RAČUN_RADNIK1` FOREIGN KEY (`RADNIK_ZAPOSLENI_JMB`) REFERENCES `radnik` (`ZAPOSLENI_JMB`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `račun`
--

LOCK TABLES `račun` WRITE;
/*!40000 ALTER TABLE `račun` DISABLE KEYS */;
/*!40000 ALTER TABLE `račun` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sadrzaj_skladista`
--

DROP TABLE IF EXISTS `sadrzaj_skladista`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sadrzaj_skladista` (
  `skladiste_idSkladista` int NOT NULL,
  `idProdukta` int NOT NULL,
  `Naziv` varchar(45) NOT NULL,
  `Vrsta` varchar(45) NOT NULL,
  `Kolicina` int NOT NULL,
  `Cena` decimal(10,2) NOT NULL,
  PRIMARY KEY (`skladiste_idSkladista`),
  UNIQUE KEY `idProdukta_UNIQUE` (`idProdukta`),
  CONSTRAINT `fk_sadrzaj_skladista_skladiste1` FOREIGN KEY (`skladiste_idSkladista`) REFERENCES `skladiste` (`idSkladista`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sadrzaj_skladista`
--

LOCK TABLES `sadrzaj_skladista` WRITE;
/*!40000 ALTER TABLE `sadrzaj_skladista` DISABLE KEYS */;
/*!40000 ALTER TABLE `sadrzaj_skladista` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `skladiste`
--

DROP TABLE IF EXISTS `skladiste`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `skladiste` (
  `idSkladista` int NOT NULL,
  PRIMARY KEY (`idSkladista`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skladiste`
--

LOCK TABLES `skladiste` WRITE;
/*!40000 ALTER TABLE `skladiste` DISABLE KEYS */;
INSERT INTO `skladiste` VALUES (1);
/*!40000 ALTER TABLE `skladiste` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `telefon`
--

DROP TABLE IF EXISTS `telefon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `telefon` (
  `BrojTelefona` varchar(20) NOT NULL,
  `NazivFirme` varchar(45) NOT NULL,
  `DOBAVLJAC_idDostavljaca` int NOT NULL,
  PRIMARY KEY (`BrojTelefona`,`DOBAVLJAC_idDostavljaca`),
  KEY `fk_TELEFON_DOBAVLJAC1_idx` (`DOBAVLJAC_idDostavljaca`),
  CONSTRAINT `fk_TELEFON_DOBAVLJAC1` FOREIGN KEY (`DOBAVLJAC_idDostavljaca`) REFERENCES `dobavljac` (`idDostavljaca`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `telefon`
--

LOCK TABLES `telefon` WRITE;
/*!40000 ALTER TABLE `telefon` DISABLE KEYS */;
/*!40000 ALTER TABLE `telefon` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `zaposleni`
--

DROP TABLE IF EXISTS `zaposleni`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zaposleni` (
  `JMB` char(12) NOT NULL,
  `Ime` varchar(20) NOT NULL,
  `Prezime` varchar(20) NOT NULL,
  `MJESTO_PostanskiBroj` int NOT NULL,
  PRIMARY KEY (`JMB`),
  KEY `fk_ZAPOSLENI_MJESTO1_idx` (`MJESTO_PostanskiBroj`),
  CONSTRAINT `fk_ZAPOSLENI_MJESTO1` FOREIGN KEY (`MJESTO_PostanskiBroj`) REFERENCES `mjesto` (`PostanskiBroj`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zaposleni`
--

LOCK TABLES `zaposleni` WRITE;
/*!40000 ALTER TABLE `zaposleni` DISABLE KEYS */;
INSERT INTO `zaposleni` VALUES ('012940194444','Mljeko','Nesto',4),('111111111111','Stipe','Stipic',1),('113454543342','Stiko','Stikic',5),('123456789101','Niko','Nikic',1),('123456789112','Ratko','Ratkic',6),('243135623521','Niko','Nikodinovic',1);
/*!40000 ALTER TABLE `zaposleni` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Final view structure for view `isporuka_dobavljaca`
--

/*!50001 DROP VIEW IF EXISTS `isporuka_dobavljaca`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `isporuka_dobavljaca` AS select `d`.`idDostavljaca` AS `ID_Dostavljaca`,`d`.`Naziv` AS `Naziv`,`d`.`Adresa` AS `Adresa`,`isporuka`.`idIsporuke` AS `ID_Isporuke` from (`dobavljac` `d` join `isporuka` on((`d`.`idDostavljaca` = `isporuka`.`DOBAVLJAC_idDostavljaca`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `isporuke_dostavljaca`
--

/*!50001 DROP VIEW IF EXISTS `isporuke_dostavljaca`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `isporuke_dostavljaca` AS select `ip`.`PRODUKT_idProdukta` AS `ID_produkta`,`p`.`Naziv` AS `Naziv`,`p`.`Vrsta` AS `Vrsta`,`ip`.`Kolicina` AS `Dostavljena_kolicina`,`p`.`Proizvodjac` AS `Proizvodjac`,date_format(`i`.`Datum`,'%d.%m.%Y') AS `Datum_isporuke` from ((((`isporuka` `i` left join `dobavljac` `d` on((`d`.`idDostavljaca` = `i`.`DOBAVLJAC_idDostavljaca`))) left join `nabavka` `n` on((`n`.`idPotvrde` = `i`.`NABAVKA_idPotvrde`))) left join `isporuka_produkta` `ip` on((`ip`.`DOSTAVA_idIsporuke` = `i`.`idIsporuke`))) left join `produkt` `p` on((`p`.`idProdukta` = `ip`.`PRODUKT_idProdukta`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `isporuke_lista`
--

/*!50001 DROP VIEW IF EXISTS `isporuke_lista`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `isporuke_lista` AS select `i`.`idIsporuke` AS `ID_isporuke`,`n`.`idPotvrde` AS `ID_nabavke`,concat(`ip`.`PRODUKT_idProdukta`,'                      ',`p`.`Naziv`,' ',`p`.`Vrsta`,'            ',`ip`.`Kolicina`,'           ',`p`.`Proizvodjac`,'           ',convert(date_format(`i`.`Datum`,'%d.%m.%Y') using utf8mb3)) AS `isplistUkupno` from ((((`isporuka` `i` left join `dobavljac` `d` on((`d`.`idDostavljaca` = `i`.`DOBAVLJAC_idDostavljaca`))) left join `nabavka` `n` on((`n`.`idPotvrde` = `i`.`NABAVKA_idPotvrde`))) left join `isporuka_produkta` `ip` on((`ip`.`DOSTAVA_idIsporuke` = `i`.`idIsporuke`))) left join `produkt` `p` on((`p`.`idProdukta` = `ip`.`PRODUKT_idProdukta`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `isporuke_nabavke`
--

/*!50001 DROP VIEW IF EXISTS `isporuke_nabavke`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `isporuke_nabavke` AS select `n`.`idPotvrde` AS `ID_nabavke`,`d`.`Naziv` AS `Dostavljac`,(case when (sum(`np`.`Cena`) is null) then 0.0 else cast(sum(`np`.`Cena`) as decimal(10,2)) end) AS `Ukupna_cena`,(case when (max(date_format(`i`.`Datum`,'%d.%m.%Y')) is null) then '-' else cast(max(date_format(`i`.`Datum`,'%d.%m.%Y')) as char(12) charset utf8mb4) end) AS `datum_posl_isp` from ((((`nabavka` `n` left join `isporuka` `i` on((`i`.`NABAVKA_idPotvrde` = `n`.`idPotvrde`))) left join `dobavljac` `d` on((`n`.`DOBAVLJAC_idDostavljaca` = `d`.`idDostavljaca`))) left join `nabavka_produkta` `np` on(((`np`.`NABAVKA_idPotvrde` = `n`.`idPotvrde`) and (`np`.`isporuka_produkta_DOSTAVA_idIsporuke` = `i`.`idIsporuke`)))) left join `isporuka_produkta` `ip` on((`ip`.`DOSTAVA_idIsporuke` = `i`.`idIsporuke`))) group by `n`.`idPotvrde` order by `n`.`idPotvrde` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `mjesto_sa_postanskimbr`
--

/*!50001 DROP VIEW IF EXISTS `mjesto_sa_postanskimbr`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `mjesto_sa_postanskimbr` AS select concat(`m`.`PostanskiBroj`,'-',`m`.`Naziv`) AS `infoMjesta` from `mjesto` `m` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `pregled_dostavljaca`
--

/*!50001 DROP VIEW IF EXISTS `pregled_dostavljaca`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `pregled_dostavljaca` AS select `d`.`idDostavljaca` AS `ID_Dostavljaca`,`d`.`Naziv` AS `Naziv`,`d`.`Adresa` AS `Adresa`,`m`.`Naziv` AS `Mjesto` from (`dobavljac` `d` left join `mjesto` `m` on((`m`.`PostanskiBroj` = `d`.`MJESTO_PostanskiBroj`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `pregled_isporuka`
--

/*!50001 DROP VIEW IF EXISTS `pregled_isporuka`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `pregled_isporuka` AS select `d`.`Naziv` AS `Dostavljac`,`n`.`idPotvrde` AS `ID_nabavke`,`ip`.`PRODUKT_idProdukta` AS `ID_produkta`,`p`.`Naziv` AS `Naziv`,`p`.`Vrsta` AS `Vrsta`,`ip`.`Kolicina` AS `Dostavljena_kolicina`,`p`.`Proizvodjac` AS `Proizvodjac`,date_format(`i`.`Datum`,'%d.%m.%Y') AS `Datum_isporuke` from ((((`isporuka` `i` left join `dobavljac` `d` on((`d`.`idDostavljaca` = `i`.`DOBAVLJAC_idDostavljaca`))) left join `nabavka` `n` on((`n`.`idPotvrde` = `i`.`NABAVKA_idPotvrde`))) left join `isporuka_produkta` `ip` on((`ip`.`DOSTAVA_idIsporuke` = `i`.`idIsporuke`))) left join `produkt` `p` on((`p`.`idProdukta` = `ip`.`PRODUKT_idProdukta`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `pregled_isporuka_nabake`
--

/*!50001 DROP VIEW IF EXISTS `pregled_isporuka_nabake`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `pregled_isporuka_nabake` AS select `p`.`idProdukta` AS `IDProdukta`,`p`.`Naziv` AS `Produkt`,`p`.`Vrsta` AS `Vrsta`,`ip`.`Kolicina` AS `Kolicina`,`p`.`Proizvodjac` AS `Proizvodjac`,(`p`.`Cena` * `ip`.`Kolicina`) AS `Cena`,date_format(`i`.`Datum`,'%d.%m.%Y') AS `Datum_isporuke`,`n`.`idPotvrde` AS `IDNabavke`,`i`.`idIsporuke` AS `IDIsporuke` from (((`produkt` `p` left join `isporuka_produkta` `ip` on((`ip`.`PRODUKT_idProdukta` = `p`.`idProdukta`))) left join `isporuka` `i` on((`i`.`idIsporuke` = `ip`.`DOSTAVA_idIsporuke`))) left join `nabavka` `n` on((`n`.`idPotvrde` = `i`.`NABAVKA_idPotvrde`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `pregled_produkta`
--

/*!50001 DROP VIEW IF EXISTS `pregled_produkta`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `pregled_produkta` AS select `p`.`idProdukta` AS `ID produkta`,concat(`p`.`Naziv`,' ',`p`.`Vrsta`) AS `produktInfo`,`p`.`Proizvodjac` AS `Proizvodjac`,`p`.`Cena` AS `Cena` from `produkt` `p` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `pregled_skladista`
--

/*!50001 DROP VIEW IF EXISTS `pregled_skladista`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `pregled_skladista` AS select `p`.`idProdukta` AS `ID_produkta`,`p`.`Naziv` AS `Naziv_produkta`,`p`.`Vrsta` AS `Vrsta_produkta`,sum(`np`.`Kolicina`) AS `Kolicina`,sum(`np`.`Cena`) AS `Cena` from (((`skladiste` `s` left join `nabavka` `n` on((`n`.`SKLADISTE_idSkladista` = `s`.`idSkladista`))) left join `nabavka_produkta` `np` on((`np`.`NABAVKA_idPotvrde` = `n`.`idPotvrde`))) left join `produkt` `p` on((`p`.`idProdukta` = `np`.`isporuka_produkta_PRODUKT_idProdukta`))) group by `p`.`idProdukta` order by `p`.`idProdukta` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `pregled_zaposlenih`
--

/*!50001 DROP VIEW IF EXISTS `pregled_zaposlenih`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `pregled_zaposlenih` AS select `z`.`JMB` AS `JMB`,`z`.`Ime` AS `Ime`,`z`.`Prezime` AS `Prezime`,`m`.`Naziv` AS `Mjesto_Stanovanja` from (((`zaposleni` `z` left join `mjesto` `m` on((`m`.`PostanskiBroj` = `z`.`MJESTO_PostanskiBroj`))) left join `menadzer` `mn` on((`mn`.`ZAPOSLENI_JMB` = `z`.`JMB`))) left join `radnik` `r` on((`r`.`ZAPOSLENI_JMB` = `z`.`JMB`))) where (`mn`.`ZAPOSLENI_JMB` is null) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-05 14:37:17
