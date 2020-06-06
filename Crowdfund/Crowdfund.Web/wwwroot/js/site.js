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

let packageSuccessAlert = $('.js-package-success-alert');
packageSuccessAlert.hide();

let packageFailAlert = $('.js-package-fail-alert');
packageFailAlert.hide();

let projectDeleteSuccessAlert = $('.js-project-delete-success-alert');
projectDeleteSuccessAlert.hide();

let projectDeleteFailAlert = $('.js-project-delete-fail-alert');
projectDeleteFailAlert.hide();

let userDeleteSuccessAlert = $('.js-user-delete-success-alert');
userDeleteSuccessAlert.hide();

let userDeleteFailAlert = $('.js-user-delete-fail-alert');
userDeleteFailAlert.hide();

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
        failAlert.html(`<i class="fa fa-exclamation-triangle"></i> User could not be updated! <br />` + failureResponse.responseText);
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
        localStorage.setItem('userId', user.userId);
    }).fail(failureResponse => {
        failAlert.html(`<i class="fa fa-exclamation-triangle"></i> User could not be created! <br />` + failureResponse.responseText);
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function deleteUser(id) {
    userDeleteSuccessAlert.hide();
    userDeleteFailAlert.hide();

    $.ajax({
        type: 'DELETE',
        url: `/user/${id}`
    }).done(response => {
        userDeleteSuccessAlert.show();
        userDeleteSuccessAlert.fadeOut(2000);
        setTimeout(function () {
            location.reload();
        }, 1000);
        
    }).fail(failureResponse => {
        userDeleteFailAlert.html(`<i class="fa fa-exclamation-triangle"></i> User could not be deleted! <br />` + failureResponse.responseText);
        userDeleteFailAlert.show();
        userDeleteFailAlert.fadeOut(3000);
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
        failAlert.html(`<i class="fa fa-exclamation-triangle"></i> Project could not be updated! <br />` + failureResponse.responseText);
        failAlert.show();
        failAlert.fadeOut(3000);
    });
}

function createProject() {
    projectSuccessAlert.hide();
    projectFailAlert.hide();

    let mediaUrl = [$('#ImageURL1').val(),
        $('#ImageURL2').val(),
        $('#ImageURL3').val(),
        $('#VideoURL').val()];

    sendData = {
        //"UserId": parseInt($('#UserId').val()),
        "UserId": parseInt(localStorage.getItem('userId')),
        "Title": $('#Title').val(),
        "Description": $('#ProjectDescription').val(),
        "Category": parseInt($('#Category').val()),
        "MediaURLs": mediaUrl,
        "FinancialGoal": parseFloat($('#FinancialGoal').val())
    }

    $.ajax({
        type: 'POST',
        url: `/project/create`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(project => {
        projectSuccessAlert.show();
        projectSuccessAlert.fadeOut(2000);
        setTimeout(function () {
            $('.new-project-form').hide();
            $('#ProjectId').val(project.projectId);
            $('.new-project-form-packages').show();
        }, 1000);
    }).fail(failureResponse => {
        projectFailAlert.html(`<i class="fa fa-exclamation-triangle"></i> Project could not be created! <br />` + failureResponse.responseText);
        projectFailAlert.show();
        projectFailAlert.fadeOut(3000);
    });
}

function addPackageToProject() {
    packageSuccessAlert.hide();
    packageFailAlert.hide();

    sendData = {
        "ProjectId": parseInt($('#ProjectId').val()),
        "Description": $('#PackageDescription').val(),
        "Price": parseFloat($('#Price').val())
    }

    $.ajax({
        type: 'POST',
        url: `/package/create`,
        contentType: 'application/json',
        data: JSON.stringify(sendData)
    }).done(project => {
        packageSuccessAlert.show();
        packageSuccessAlert.fadeOut(3000);
    }).fail(failureResponse => {
        packageFailAlert.html(`<i class="fa fa-exclamation-triangle"></i> Package could not be created! <br />` + failureResponse.responseText);
        packageFailAlert.show();
        packageFailAlert.fadeOut(3000);
    });
}

$('.js-go-to-project').on('click', () => {
    projectid = parseInt($('#ProjectId').val())
    window.location.replace(`/project/${projectid}/details`);
})

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
        projectDeleteFailAlert.html(`<i class="fa fa-exclamation-triangle"></i> User could not be created! <br />` + failureResponse.responseText);
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
    localStorage.setItem('userId', id);
}

function purchasePackage(packageId) {
    var userId = localStorage.getItem('userId');

    $.ajax({
        type: 'POST',
        url: `/package/${packageId}/purchase/${userId}`
    }).done(successResponse => {
        successAlert.html(`<i class="fa fa-check"></i> User with ID=${userId} has purchaged package with ID=${packageId}`)
        successAlert.show();
        successAlert.fadeOut(3000);
    }).fail(failureResponse => {
        failAlert.html(`<i class="fa fa-exclamation-triangle"></i> Package could not be purchased,maybe you already purchased this package OR you just need to virtual login <br />`);
        failAlert.show();
        failAlert.fadeOut(3000);
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

function goToCreateProject(userId) {
    localStorage.setItem('userId', userId);
    window.location.replace(`/project/create`);
}