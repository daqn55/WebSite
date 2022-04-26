$("#categoryList").find("button").click(onClickButtonDelete);

function onClickButtonDelete(e) {
    let stringId = e.target.id;
    let splitStringId = stringId.split('-');
    let id = splitStringId[2];
    let typeOfCat = splitStringId[1];
    console.log(typeOfCat);
    console.log(id);

}