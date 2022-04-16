$(document).ready(function () {
    $("body").tooltip({ selector: '[data-toggle=tooltip]' });
});

function ShowModal(id) {
    $('#' + id).modal('show')
}

function ShowModalStatic(id) {
    $('#' + id).modal({backdrop: 'static', keyboard: false})
}

function HideModal(id) {
    $('#' + id).modal('hide')
}

function HideNavbar(id) {
    $('#' + id).collapse('hide')
}