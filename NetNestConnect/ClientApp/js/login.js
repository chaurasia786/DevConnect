var loginApp = angular.module('loginApp', []);

loginApp.controller('loginController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    $scope.email = "";
    $scope.password = "";

    $scope.submitLogin = function () {
        /*var recaptchaResponse = grecaptcha.getResponse();*/
        // Endpoint matches the .NET Core Login route
        var endpoint = 'https://localhost:7085/api/User/Login';

        $http({
            method: 'POST',
            url: endpoint,
            data: {
                Email: $scope.email,
                Password: $scope.password,
                recaptchaResponse: ''
            },
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            if (response.data.isSuccess) {
                // If the login is successful
                // You might also want to save the token somewhere, e.g., in the session storage
                sessionStorage.setItem("token", response.data.token);
                $window.location.href = 'index.html';  // redirect to main page
            } else {
                // Handle unsuccessful login
                alert("Incorrect credentials. Please try again.");
                $scope.errorMessage = "Incorrect credentials. Please try again.";
            }
        }, function (error) {
            console.error("Error during login:", error);
            $scope.errorMessage = "Error during login. Please check the console for details.";
        });
    };
}]);
