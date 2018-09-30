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
                    JsMain.ShowLoaddingIndicator();
                    
                },
                complete: function (data) {
                    setTimeout(
                        function () {
                            JsMain.HideLoaddingIndicator();
                        }, 5000);
                },
                success: function (data) {
                    $("#globalHTMLAppender").html(data);
                },
                error: function (data) {
                    JsMain.HideLoaddingIndicator();
                    JsMain.Response404Error(data);
                }
            });

        }
        publicMethod.Response404Error1 = function (data) {

            var urll = "\AccessDenied";

            window.location.href = urll;
        }
        publicMethod.Response404Error = function (httpObj, data) {
          
            var urll = "\AccessDenied";
           
            window.location.href = urll;
        }

        publicMethod.RedirectToHomePage = function () {
            JsMain.ShowLoaddingIndicator();
            var url = "\Home";

            window.location.href = url;
        }


    }(window.JsMain = window.JsMain || {}, jQuery)
);
