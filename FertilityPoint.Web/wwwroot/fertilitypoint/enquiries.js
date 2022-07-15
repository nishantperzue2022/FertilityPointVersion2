


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

        var tr = '';

        $.ajax({

            url: '/Admin/Enquiries/GetEnquiries',
            method: 'Get',
            success: (result) => {

                $.each(result, (k, v) => {

                    tr += `<tr>
                    <td>${v.name}</td>
                    <td>${v.message}</td>
                    <td>${v.phoneNumber}</td>
                    <td>${v.status}</td>         
               

                       </tr>`
                })
                $("#tblEnquiries").html(tr);
            },
            error: (error) => {
                console.log(error);
            }
        });

    }

})

