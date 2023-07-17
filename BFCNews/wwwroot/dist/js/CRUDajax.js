$(document).ready(function () {
    $("#btnAdd").click(function(){
        var name = $("#Department").val().toString();
        if (name != null) {
            $.ajax({
                url: '/Department/Add',
                type: "POST",
                data: { "name": name },
                success: function (response) {
                    alert(response);
                    if (response != null) {
                        alert("Name : " + response.Name + ", Designation : " + response.Designation + ", Location :" + response.Location);
                    } else {
                        alert("Something went wrong");
                    }
                }

            })
        }
    });
})