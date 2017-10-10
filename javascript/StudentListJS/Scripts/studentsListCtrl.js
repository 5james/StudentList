app.controller("studentsListCtrl", function ($scope, $http, NgTableParams) {

    var self = this;

    /* ------------------VARIABLES------------------ */
    /* ---------PUBLIC--------- */
    //Tables
    $scope.studentsListTable = new NgTableParams({ count: 5 }, { counts: [5, 10, 20] });
    $scope.groupsListTable = new NgTableParams({ count: 5 }, { counts: [5, 10, 20] });

    $scope.bDisplayError = false;
    $scope.errorMessage = "";

    $scope.selectedStudent = {};
    $scope.selectedGroup = {};


    /* ------------------FUNCTIONS------------------ */
    /* ---------PUBLIC--------- */
    $scope.putStudentInForms = function (studID) {
        for (var i = 0; i < $scope.studentsListTable.data.length; ++i) {
            if ($scope.studentsListTable.data[i].IDStudent === studID) {
                $scope.selectedStudent = {
                    IDGroup: $scope.studentsListTable.data[i].IDGroup,
                    IDStudent: $scope.studentsListTable.data[i].IDStudent,
                    FirstName: $scope.studentsListTable.data[i].FirstName,
                    LastName: $scope.studentsListTable.data[i].LastName,
                    IndexNo: $scope.studentsListTable.data[i].IndexNo,
                    BirthPlace: $scope.studentsListTable.data[i].BirthPlace,
                    BirthDate: $scope.studentsListTable.data[i].BirthDate,
                    Stamp: $scope.studentsListTable.data[i].Stamp
                };

            }
        }
        console.log(studID);
        console.log($scope.studentsListTable);
        console.log($scope.selectedStudent);
        $scope.formStudent.$setPristine();
    }

    $scope.putGroupInForms = function (grID) {
        for (var i = 0; i < $scope.groupsListTable.data.length; ++i) {
            if ($scope.groupsListTable.data[i].IDGroup === grID) {
                $scope.selectedGroup = {
                    IDGroup: $scope.groupsListTable.data[i].IDGroup,
                    Name: $scope.groupsListTable.data[i].Name,
                    Stamp: $scope.groupsListTable.data[i].Stamp
                };

            }
        }
        console.log($scope.selectedGroup);
        $scope.formGroup.$setPristine();
    }

    $scope.clearFilters = function () {
        console.log($scope.studentsListTable.filter())
        console.log($scope.studentsListTable)
        $scope.studentsListTable.filter({});
    }
    $scope.dismissErrorMessage = function () {
        $scope.bDisplayError = false;
        $scope.errorMessage = "";

    }
    $scope.getStudentsAndGroups = function () {
        $http({
            url: '/api/Students',
            method: 'GET'
        }).then(successCallback, errorCallback);
    }

    $scope.createStudent = function () {
        console.log("create student");

        $http({
            url: '/api/Students/POST',
            method: 'POST',
            data: JSON.stringify({
                FirstName: $scope.selectedStudent.FirstName,
                LastName: $scope.selectedStudent.LastName,
                IndexNo: $scope.selectedStudent.IndexNo,
                IDGroup: $scope.selectedStudent.IDGroup,
                BirthPlace: $scope.selectedStudent.BirthPlace,
                BirthDate: $scope.selectedStudent.BirthDate
            })
        }).then(successCallback, errorCallback);
    }
    $scope.updateStudent = function () {
        console.log("update student");
        $http({
            url: '/api/Students/PUT',
            method: 'POST',
            data: JSON.stringify($scope.selectedStudent)
        }).then(successCallback, errorCallback);
    }
    $scope.deleteStudent = function () {
        console.log("delete student");
        $http({
            url: '/api/Students/DELETE',
            method: 'POST',
            data: JSON.stringify($scope.selectedStudent)
        }).then(successCallback, errorCallback);
    }

    $scope.createGroup = function () {
        console.log("create group");
        $http({
            url: '/api/Groups/POST',
            method: 'POST',
            data: JSON.stringify({
                Name: $scope.selectedGroup.Name
            })
        }).then(successCallback, errorCallback);
    }
    $scope.updateGroup = function () {
        console.log("update group");
        $http({
            url: '/api/Groups/PUT',
            method: 'POST',
            data: JSON.stringify($scope.selectedGroup)
        }).then(successCallback, errorCallback);
    }
    $scope.deleteGroup = function () {
        console.log("delete group");
        $http({
            url: '/api/Groups/DELETE',
            method: 'POST',
            data: JSON.stringify($scope.selectedGroup)
        }).then(successCallback, errorCallback);
    }
    /* ---------PRIVATE--------- */
    function successCallback(response) {
        console.log("success");
        $scope.dismissErrorMessage();
        if (response.data.groupslist) {
            if (response.data.studentslist) {
                updateStudents(response.data.studentslist, response.data.groupslist);
            }
            updateGroups(response.data.groupslist);
            if (response.data.errormsg) {
                $scope.bDisplayError = true;
                $scope.errorMessage = response.data.errormsg;
            }
        }
        else {
            $scope.bDisplayError = true;
            $scope.errorMessage = "Wystąpił błąd podczas pobierania danych.";
        }
    }

    function errorCallback(response) {
        console.log("error");
        $scope.bDisplayError = true;
        $scope.errorMessage = "Wystąpił błąd (" + response.data.errormsg + ")";
    }

    function updateStudents(students, groups) {
        console.log("update student");
        $scope.selectedStudent = {};

        console.log($scope.studentsListTable);

        var dict = {};

        for (var i = 0; i < groups.length; ++i) {
            var key = groups[i].IDGroup.toString();
            dict[key] = groups[i].Name;
        }
        var tmpStudents = students;
        console.log(dict);
        for (var i = 0; i < tmpStudents.length; ++i) {
            tmpStudents[i].GroupName = dict[tmpStudents[i].IDGroup];
        }

        var page = $scope.studentsListTable.page();

        $scope.studentsListTable.settings().dataset = tmpStudents;
        $scope.studentsListTable.reload();

        console.log(tmpStudents);
        console.log($scope.studentsListTable);

        $scope.studentsListTable.page(page);
    }

    function updateGroups(groups) {
        console.log("update group");
        //$scope.groupsList = groups;
        console.log($scope.groupsListTable);
        console.log(groups);
        $scope.selectedGroup = {};

        var page = $scope.groupsListTable.page();

        $scope.groupsListTable.settings().dataset = groups;
        $scope.groupsListTable.reload();

        $scope.groupsListTable.page(page);
        console.log($scope.groupsListTable);
    }


    /* ---INIT--- */
    $scope.getStudentsAndGroups();
});