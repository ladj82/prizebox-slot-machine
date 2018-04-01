<?php

usleep(1000000);

$data = array(
	'result' => false
);

header('Content-type:application/json;charset=utf-8');

echo json_encode($data);

?>