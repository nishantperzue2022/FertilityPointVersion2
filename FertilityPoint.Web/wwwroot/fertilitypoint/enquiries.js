

$(() => {

    LoadEnquiriesData();

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalrServer").build();

    connection.start();

    connection.on("LoadEnquiries", function () {

        LoadEnquiriesData();
    })

    LoadEnquiriesData();

    function LoadEnquiriesData() {

        $.ajax({
            type: "GET",
            url: "/Admin/Enquiries/GetEnquiries/",
            data: "{ }",

            success: function (data) {

                const array = data;

                console.log(data);

                if (array.length == 0) {

                    $('#divShowEmptyMessage').show();

                } else {
                    $('#divShowEmptyMessage').hide();

                    var s = '';


                    for (var i = 0; i < array.length; i++) {

                        s +=
                            `<a href="#"  onclick="GetMessage('${data[i].id}')">
                                        <div class="mail_list">
                                            <div class="left">
                                                <i class="fa fa-circle"></i>
                                            </div>
                                            <div class="right" >
                                                <h3 style="color:#27ae60;">${data[i].name} <small>${data[i].newCreateDate}</small></h3>
                                                <span>  <small ></small></span>
                                                <p>
                                                    Phone Number:<span >${data[i].phoneNumber}</span><br>

                                                    Email :<span id="txtEmail">${data[i].email}</span>

                                                </p>

                                            </div>
                                        </div>
                                    </a>`
                            ;

                        $("#txtlist").html(s);

                    }
                }
            }

        });
    }
})


function GetMessage(e) {

    $('#divReply').show();

    var id = e;

    console.log(id);

    $.get("/Admin/Enquiries/GetById/?Id=" + id, function (data, status) {
        console.log(data);
        if (data.data == false) {
            alert("Does not exist");

            console.log(data);

        } else {

            $("#txtId").val(data.data.id);
            $("#txtMessage").text(data.data.message);
            $("#txtPhoneNumber").text(data.data.phoneNumber);
            $("#txtEmail1").text(data.data.email);
            $("#txtStatus").text(data.data.status);
            $("#txtName").text(data.data.name);
        }

    });
};

function ShowLoader() {

    $("#loadMe").modal('show');
}

function HideLoader() {

    $("#loadMe").modal('hide');
}

$("#btnReply").click(function () {



    if ($('#txtComposeMail').val() == '') {
        $('#txtComposeMail').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Message is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    var data = $("#frmReply").serialize();

    $.ajax({

        type: "POST",

        url: "/Admin/Enquiries/SendMail/",

        data: data,

        beforeSend: function () { ShowLoader(); },

        success: function (response) {

            if (response.success) {


                swal({

                    position: 'top-end',

                    type: "success",

                    title: response.responseText,

                    showConfirmButton: false,

                }), setTimeout(function () { location.reload(); }, 3000);

            } else {

                swal({

                    position: 'top-end',

                    type: "error",

                    title: response.responseText,

                    showConfirmButton: true,

                    timer: 5000,
                });


            }
        },

        error: function (response) {
            alert("error!");
        },
        complete: function () {
            HideLoader();
        }
    })

})




