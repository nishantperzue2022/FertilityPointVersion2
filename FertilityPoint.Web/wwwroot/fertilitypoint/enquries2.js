

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
            url: "/Admin/Enquiries/GetById2/",
            data: "{ }",

            success: function (data) {

                const array = data

                var s = '';

                console.log(data);

                for (var i = 0; i < data.length; i++) {

                    s +=
                        `<a href="#"  onclick="GetMessage('${data[i].makeId}')">
                                        <div class="mail_list">
                                            <div class="left">
                                                <i class="fa fa-circle"></i>
                                            </div>
                                            <div class="right" >
                                                <h3 style="color:#27ae60;">${data[i].makeName} <small>${data[i].newCreateDate}</small></h3>
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
        } else {

            $("#txtId").val(data.data.id);
            $("#txtMessage").text(data.data.message);
            $("#txtPhoneNumber").text(data.data.phoneNumber);
            $("#txtEmail").text(data.data.email);
            $("#txtStatus").text(data.data.status);
            $("#txtName").text(data.data.name);
        }

    });
};