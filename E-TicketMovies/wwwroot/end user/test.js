$(document).ready(function () {
    $.ajax({
        url: '/End%20User/Cart/GetCartCount',
        type: 'GET',
        success: function (data) {
            console.log("✅ Cart count received:", data);
            $('#lblCartCount').text(data);
        },
        error: function (xhr, status, error) {
            console.error("❌ AJAX error:", error);
            console.log(xhr.responseText);
        }
    });
});
