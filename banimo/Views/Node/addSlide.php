
<?php

include_once ("../../../portal/classes/PartoPortal2.php");
$Mabbdaco = new PartoPortal();
$catID = $Mabbdaco->post("catID");
$locationID = $Mabbdaco->post("locationID");
$device = $Mabbdaco->post("device");
$code = $Mabbdaco->post("code");
$token = $Mabbdaco->post("token");
$type = $Mabbdaco->post("type");
$image = $Mabbdaco->post("image");

if ($code != md5($device . "ncase8934f49909")) {
    echo $code;
    die("InvalidRequest");
}

$where  = "1 = 1 ";
if ($catID != ""){
    $where = $where. " and catID = $catID ";
}
if ($locationID != ""){
    $where = $where. " and locationID = $locationID ";
}

$query = "DELETE FROM `mbd_slides` WHERE $whereand type = $type  ";
  echo($query."\n");
$q = mysqli_query($Mabbdaco->db_conn, $query) or die(mysqli_error($Mabbdaco->
    db_conn));

$list = explode(",", $image);
foreach ($list as $item) {
    $query = "select * from  `mbd_slides`  where  $where and type = $type  and image = '$item'";
    echo($query."\n");
    $isrowexist = $Mabbdaco->getNumROWS($query);
    if ($isrowexist == 0) {
        if ($item != '') {
            $query = "INSERT INTO  `mbd_slides`  values(null,'$item',$catID,$locationID,$type)";
              echo($query."\n");
            $q = mysqli_query($Mabbdaco->db_conn, $query) or die(mysqli_error($Mabbdaco->
                db_conn));
        }

    }


}





?>
