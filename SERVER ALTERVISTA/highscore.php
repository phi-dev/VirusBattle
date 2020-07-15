<?php

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

$sql = "SELECT DISTINCT players.name,highscore.score FROM highscore JOIN players ON players.deviceId = highscore.deviceId ORDER BY score DESC LIMIT 10 ";
$result = $conn->query($sql);
$scores = array();

if ($result->num_rows > 0) {
  // output data of each row
  while($row = $result->fetch_assoc()) {
    array_push($scores,$row);
  }
}
$conn->close();

echo json_encode($scores);

?>