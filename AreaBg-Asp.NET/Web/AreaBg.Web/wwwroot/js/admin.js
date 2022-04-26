$("#checkAll").click(function () {
    $('[name="orderCheck"]').prop('checked', true)
});

$("#uncheckAll").click(function () {
    $('[name="orderCheck"]').prop('checked', false)
});

$("[name='refuse-status']").click(function (e) {
    e.preventDefault()
    let id = $(e.target).closest('a').attr('id');

    $.ajax({
        type: 'GET',
        url: '/Admin/Order/OrderRefuse?id=' + id,
        success: function (result) {
            //TODO: съобщение??
            location.reload()
        }
    })
})

$("[name='refuse-status-with-mail']").click(function (e) {
    e.preventDefault()
    let id = $(e.target).closest('a').attr('id');

    $.ajax({
        type: 'GET',
        url: '/Admin/Order/OrderRefuseMail?id=' + id,
        success: function (result) {
            //TODO: съобщение??
            location.reload()
        }
    })
})

$("[name='change-status']").click(function (e) {
    e.preventDefault()
    let id = $(e.target).closest('a').attr('id');

    changeStatus(id)
})

function changeStatus(id) {
    $.ajax({
        type: 'GET',
        url: '/Admin/Order/OrderSend?id=' + id,
        success: function (result) {
            //TODO: съобщение??
            location.reload()
        }
    })
}

$('[name="reply-message"]').click(function (e) {
    let btn = $(e.target)
    let id = btn.attr('id')

    $.ajax({
        type: 'GET',
        url: '/Admin/Mails/ArchiveMail?id=' + Number(id),
        success: function (result) {
            console.log(result)
        }
    })

    alertBoxShow('true', 'Успешно архивирахте съобщението')
    $('#' + id).remove();
})

$('[name="delete-message"]').click(function(e) {
    let btn = $(e.target)
    let id = btn.attr('id')

    $.ajax({
        type: 'GET',
        url: '/Admin/Mails/RemoveMail?id=' + Number(id),
        success: function (result) {
            console.log(result)
        }
    })

    alertBoxShow('true', 'Успешно изтрихте съобщението')
    $('#' + id).remove();
})

//reviews
$('[name="approve-review"]').click(function (e) {
    let btn = $(e.target)
    let id = btn.attr('id')

    $.ajax({
        type: 'GET',
        url: '/Admin/Review/ApproveReview?id=' + Number(id),
        success: function (result) {
            console.log(result)
        }
    })

    alertBoxShow('true', 'Успешно одобрихте коментара')
    $('#' + id).remove();
})

$('[name="delete-review"]').click(function (e) {
    let btn = $(e.target)
    let id = btn.attr('id')

    $.ajax({
        type: 'GET',
        url: '/Admin/Review/RemoveReview?id=' + Number(id),
        success: function (result) {
            console.log(result)
        }
    })

    alertBoxShow('true', 'Успешно изтрихте коментара')
    $('#' + id).remove();
})