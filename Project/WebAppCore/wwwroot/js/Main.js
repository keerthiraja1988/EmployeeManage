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

        publicMethod.ShowMessageShowReloadPopUp = function (header, message) {
            $('#modalMessageShowReloadPopUpHeaderTitle').text(header);
            $('#modalMessageShowReloadPopUpMessage').text(message);
            $('#modalMessageShowReloadPopUp').modal('show');
        },

            publicMethod.ShowMessageShowPopUp = function (header, message) {
            $('#modalMessageShowPopUpHeaderTitle').text(header);
            $('#modalMessageShowPopUpMessage').text(message);
            $('#modalMessageShowPopUp').modal('show');
        },

        publicMethod.ShowMessageShowPopUp1 = function (data) {
            var splitedDtata = data.split("|");
            if (splitedDtata[1]) {
                $('#modalMessageShowPopUpHeaderTitle').text(splitedDtata[2]);
                $('#modalMessageShowPopUpMessage').text(splitedDtata[3]);
                $('#modalMessageShowPopUp').modal('show');
            }
            
        }

    }(window.JsMain = window.JsMain || {}, jQuery)
);
