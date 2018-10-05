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
            JsMain.ShowLoaddingIndicator();
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json",
                dataType: "json",
                begin: function (data) {
                    JsMain.ShowLoaddingIndicator();
                    
                },
                complete: function (data) {
                  
                },
                success: function (data) {
                   
                    setTimeout(
                        function () {
                            $("#globalHTMLAppender").html(data);
                            JsMain.HideLoaddingIndicator();
                        }, 100);
                },
                error: function (data) {
                    JsMain.HideLoaddingIndicator();
                    JsMain.Response404Error(data);
                }
            });

        }
        publicMethod.Response404Error1 = function (data) {
            window.location.href = errorPageUrl;
        }
        publicMethod.Response404Error = function (httpObj, data) {
            window.location.href = errorPageUrl;
        }

        publicMethod.RedirectToHomePage = function () {
            JsMain.ShowLoaddingIndicator();
            var url = "\Home";

            window.location.href = url;
        }


    }(window.JsMain = window.JsMain || {}, jQuery)
);
