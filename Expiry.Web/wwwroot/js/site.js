$('#deleteModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    var reminderId = button.data('reminder-id');
    $("#reminderId").val(reminderId);
});

$('#deleteConfirmation').on('click', function (event) {
    var reminderId = $("#reminderId").val();
    $.post("/Home/Delete/" + reminderId, function () {
        $('#deleteModal').modal('hide');
        window.location.href = '/Home';
    }).fail(function (error) {
        console.log(error)
        alert("Se ha producido un error");
    });
});