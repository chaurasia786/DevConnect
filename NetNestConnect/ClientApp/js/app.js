var myApp = angular.module('myApp', ['ngMessages']);

myApp.controller('myController', function($scope, $http)
{
	console.log("Hello In myContoller...");	
	$scope.newUser = {
		id: 0,
		firstName: '',
		lastName: '',
		email: '',
		phoneNumber: '',
		password: ''
	};
	$scope.checkedUser = {};
	$scope.message = "";

	$scope.hasError = function (field) {
		return field.$touched && !angular.equals({}, field.$error);
	};

	$http.get('https://localhost:7085/api/User/AllUsers')
    .then(function(response) {
        console.log(response.data);
        $scope.users = response.data;
        console.log($scope.users);  // Moved inside the callback
    }, function(error) {
        console.error('Error fetching users:', error);
	});

	$scope.fetchUsers = function () {
		$http.get('https://localhost:7085/api/User/AllUsers').then(function (response) {
			$scope.users = response.data;
		}, function (error) {
			alert('Error fetching users: ' + error.message);
		});
	};

	//
	//$scope.saveUser = function (user) {
	//	$http.post('https://localhost:7085/api/User/RegisterUser', user).then(function (response) {
	//		// Assuming the response contains the saved user
	//		if (response.data.IsAdded) {
	//			/*$scope.users.push(user);*/
	//			$scope.newUser = {};
	//			$scope.message = "User added successfully!";
	//		} else {
	//			$scope.message = "Error: " + response.data.ErrorMessage;
	//		}
	//	}, function (error) {
	//		$scope.message = "Error: " + error.data.errorMessage;
	//	});
	//};


	$scope.saveUser = function (user, event) {
		event.preventDefault();
		$http.post('https://localhost:7085/api/User/RegisterUser', user).then(function (response) {
			if (response.data.isAdded) {
				$scope.newUser = {};
				$scope.message = "User added successfully!";
				setTimeout(function () {
					$('#add').modal('hide');
				}, 100);
				$scope.fetchUsers();
			} else {
				$scope.message = "Error(s): " + response.data.ErrorMessage.join(", ");
				alert($scope.message);
			}
		}, function (error) {
			$scope.message = "Error(s): " + error.data.errorMessage.join(", ");
			alert($scope.message);
		});
	};

	
	


	

	$scope.selectUser = function (user) {
		console.log(user);
		$scope.clickedUser = user;
		/*$http.get('https://localhost:7085/api/User?id=' + user.Id).then(function (response) {*/
		$http.get('https://localhost:7085/api/User?id=' + user.id).then(function(response) {
            if(response.data) {
                $scope.clickedUser = response.data;
            } else {
                alert('Error fetching user details');
            }
        }, function(error) {
            alert('Error fetching user details: ' + error.message);
        });
	};

	$scope.updateUser = function (user) {
		$http.post('https://localhost:7085/api/User/Update', user).then(function (response) {
			if (response.data.isUpdated) {
				alert('User updated successfully!');
				
				$scope.clickedUser = {};
				$scope.fetchUsers();
			} else {
				alert('Error updating user: ' + response.data.ErrorMessage);
			}
		}, function (error) {
			alert('Error updating user: ' + error.message);
		});
	};

	$scope.deleteUser = function () {
		$http({
			method: 'DELETE',
			url: 'https://localhost:7085/api/User?id=' + $scope.clickedUser.id
		}).then(function (response) {
			if (response.data.isDeleted) {
				alert('User deleted successfully!');

				// Remove the deleted user from the users array
				var index = $scope.users.indexOf($scope.clickedUser);
				if (index !== -1) {
					$scope.users.splice(index, 1);
				}
				$scope.clickedUser = {};  // Clear the clickedUser object
				$scope.fetchUsers();
			} else {
				alert('Error deleting user: ' + response.data.ErrorMessage);
			}
		}, function (error) {
			alert('Error deleting user: ' + error.message);
		});
	};

	$scope.logout = function () {
		$http.post('https://localhost:7085/api/User/Logout')
			.then(function (response) {
				console.log(response.data.message);
			});
		localStorage.removeItem('token');

		window.location.href = 'https://localhost:7085/ClientApp/login.html';
	}


});