(
    function (publicMethod, $) {

        publicMethod.ShowLoaddingIndicator = function () {
            $('#loadingIconModal').modal('show');
        },

        publicMethod.HideLoaddingIndicator = function () {
            setTimeout(
                function () {
                    $('#loadingIconModal').modal('hide');
                }, 500);
        }

        publicMethod.GetLoggedInUserDetails = function (url) {
           // alert(url);

            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json",
                dataType: "json",
                begin: function (data) {

                },
                complete: function (data) {
                    //alert(data);
                  
                },
                success: function (data) {
                    $("#globalHTMLAppender").html(data);
                }
            });

        }

        publicMethod.Response404Error = function (httpObj, data) {
          
            var urll = "\AccessDenied";
           
            window.location.href = urll;
        }


    }(window.JsMain = window.JsMain || {}, jQuery)
);
