// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let successAlert = $('.js-success-alert');
successAlert.hide();

let failAlert = $('.js-fail-alert');
failAlert.hide();

let projectSuccessAlert = $('.js-project-success-alert');
projectSuccessAlert.hide();

let projectFailAlert = $('.js-project-fail-alert');
projectFailAlert.hide();

let projectDeleteSuccessAlert = $('.js-project-delete-success-alert');
projectDeleteSuccessAlert.hide();

let projectDeleteFailAlert = $('.js-project-delete-fail-alert');
projectDeleteFailAlert.hide();

$('.new-project-form-packages').hide();

$('#staticBackdrop').on('hide.bs.modal', function (e) {
    var $if = $(e.delegateTarget).find('iframe');
    var src = $if.attr("src");
    $if.attr("src", '/empty.html');
    $if.attr("src", src);
});

function editUser() {
    successAlert.hide();
    failAlert.hide();

    userid = $('#UserId');

    sendData = {
        "FirstName": $('#FirstName').val(),
        "LastName": $('#LastName').val(),
        "Email": $('#Email').val()
    }

    $.ajax({
        type: 'PATCH',
        url: `/user/${userid.text()}`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(user => {
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function submitNewUser() {
    successAlert.hide();
    failAlert.hide();

    sendData = {
        "FirstName": $('#FirstName').val(),
        "LastName": $('#LastName').val(),
        "Email": $('#Email').val()
    }

    $.ajax({
        type: 'POST',
        url: `/user/create`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(user => {
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function editProject() {
    successAlert.hide();
    failAlert.hide();

    let mediaUrl = [$('#ImageURL1').val(),
            $('#ImageURL2').val(),
            $('#ImageURL3').val(),
        $('#VideoURL').val()];

    projectid = $('#ProjectId');
    sendData = {
        "Title": $('#Title').val(),
        "Description": $('#Description').val(),
        "Category": parseInt($('#Category').val()),
        "MediaURLs": mediaUrl
    }
    $.ajax({
        type: 'PATCH',
        url: `/project/${projectid.text()}`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(project => {
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function createProject() {
    successAlert.hide();
    failAlert.hide();

    let mediaUrl = [$('#ImageURL1').val(),
        $('#ImageURL2').val(),
        $('#ImageURL3').val(),
        $('#VideoURL').val()];

    sendData = {
        "UserId": parseInt($('#UserId').val()),
        "Title": $('#Title').val(),
        "Description": $('#Description').val(),
        "Category": parseInt($('#Category').val()),
        "MediaURLs": mediaUrl,
        "FinancialGoal": parseFloat($('#FinancialGoal').val())
    }
    alert(JSON.stringify(sendData));
    $.ajax({
        type: 'POST',
        url: `/project/create`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(project => {
        successAlert.show();
        successAlert.fadeOut(2000).delay(1000);
        $('.new-project-form').hide();
        $('#ProjectId').val(project.projectId);
        $('.new-project-form-packages').show();
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function addPackageToProject() {
    successAlert.hide();
    failAlert.hide();

    sendData = {
        "ProjectId": parseInt($('#ProjectId').val()),
        "Description": $('#Description').val(),
        "Price": parseFloat($('#Price').val())
    }

    $.ajax({
        type: 'POST',
        url: `/package/create`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(project => {
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function deleteProject(id) {
    projectDeleteSuccessAlert.hide();
    projectDeleteFailAlert.hide();
    $.ajax({
        type: 'DELETE',
        url: `/project/${id}/delete`
    }).done(project => {
        $(`#Project-${id}`).remove();
        projectDeleteSuccessAlert.show();
        projectDeleteSuccessAlert.fadeOut(3000);
    }).fail(failureResponse => {
        projectDeleteFailAlert.show();
        projectDeleteFailAlert.fadeOut(3000);
    });
}

$('#NewStatusUpdateModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget); // Button that triggered the modal
    var title = button.data('title');
    var id = button.data('id');// Extract info from data-* attributes

    var modal = $(this);
    modal.find('.modal-title').text('New status update for: ' + title);
    modal.find('.modal-body input').val(id);

    $('.js-project-newstatus-success-alert').hide();
    $('.js-project-newstatus-fail-alert').hide();

    modal.find('.js-post-update-button').on('click', () => {

        sendData = {
            "StatusDescription": $('#status-text').val()
        }

        $.ajax({
            type: 'POST',
            url: `/project/${id}/postStatus`,
            contentType: 'application/json',
            data: JSON.stringify(sendData)
        }).done(projectstatus => {
            $('.js-project-newstatus-success-alert').show();
            $('.js-project-newstatus-success-alert').fadeOut(3000);
        }).fail(failureResponse => {
            $('.js-project-newstatus-fail-alert').show();
            $('.js-project-newstatus-fail-alert').fadeOut(3000);
        });
    });
});

function virtualLogin(id) {
    debugger;
    localStorage.setItem('userId', id);
}

function purchasePackage(packageId) {
    var userId = localStorage.getItem('userId');

    $.ajax({
        type: 'POST',
        url: `/package/${packageId}/purchase/${userId}`
    }).done(successResponse => {
        alert(`${userId} has purchaged package ${packageId}`)
    }).fail(failureResponse => {
        alert('Something went wrong,maybe you already purchased this package OR you just need to virtual login')
    });
}

let searchBox = $('#searchBox');
searchBox.on("keyup", function (event) {
    if (event.keyCode === 13) {
        $('#searchButton').click();
    }
});

let searchButton = $('#searchButton');
searchButton.on('click', () => {
    let text = $('#searchBox').val()
    location.replace(`/project/search?Title=${text}`)
});

