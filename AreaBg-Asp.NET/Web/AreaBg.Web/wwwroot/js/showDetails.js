$("[id='bookInfoBtn']").click(bookInfo)

function bookInfo(e) {
    e.preventDefault();

    let uri = 'Book/BookDetails'
    let productId = $(e.target).attr('name');

    
    $.getJSON(uri + '/' + productId, function (json) {
        console.log(json);
        $("#productTitle").text(json.title);
    });
}

function searchProducts(e) {
    e.preventDefault();
    console.log($('#tableSearch tbody').remove());

    let uri = 'Admin/Home/SearchProduct'
    let productToSearch = $('#productToSearch').val();
    let searchOption = $('#searchOption').val();

    $.getJSON(uri + '/' + productToSearch + '/' + searchOption, function (json) {
        console.log(json.books[0]);
    });
}