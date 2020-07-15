<?php

$deviceId = $_POST["deviceId"];
$score = (int) $_POST["score"];

$servername = "localhost";
$username = "[HIDDEN]";
$password = "";
$dbname = "my_sharebox";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO highscore (deviceId, score) VALUES ('$deviceId', $score)";
$conn->query($sql);

$conn->close();
?>