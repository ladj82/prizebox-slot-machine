<?php

usleep(1000000);

$data = array(
	'result' => true
);

header('Content-type:application/json;charset=utf-8');

echo json_encode($data);

?>