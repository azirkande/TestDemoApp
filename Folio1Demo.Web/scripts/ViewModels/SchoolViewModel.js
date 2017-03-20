
var schoolViewModel;

function classViewModel(id, name, teacher, location) {
    var self = this;
    self.Id = id;
    self.Name = name;
    self.Teacher = teacher;
    self.Location = location;
}

function classListViewModel() {
    var self = this;

    self.classes = ko.observableArray();
    self.classId = ko.observable(0);
    self.className = ko.observable("");
    self.classTeacher = ko.observable("");
    self.classLocation = ko.observable("");
    self.isEdit = ko.observable(true);
    self.editClass = function (editClass) {
        self.classId(editClass.Id);
        self.className(editClass.Name);
        self.classId(editClass.Id);
        self.classTeacher(editClass.Teacher);
        self.classLocation(editClass.Location);
        self.isEdit(true);
    }
    self.removeClass = function (removeClass) {
        var isConfirmed = confirm("Are you sure want to delete class?");

        if (isConfirmed) {
            $.ajax({
                url: '/api/class/remove/' + removeClass.Id,
                cache: false,
                type: 'POST',
                error: function (response) {
                    alert("error occurred while deleting class.");
                },
                success: function (data) {
                    $.when(self.getClassess("remove")).then(function (data, textStatus, jqXHR) {
                        schoolViewModel.studentListViewModel.getStudents(removeClass.Id);
                    });

                }
            });
        }
    }

    self.getStudentsOnClassSelection = function (selectedClass) {
        schoolViewModel.studentListViewModel.getStudentsOnClassSelection(selectedClass);
    }

    self.addClass = function () {
        self.isEdit(false);
        self.classId(0);
        self.className("");
        self.classTeacher("");
        self.classLocation("");
    }

    self.saveClass = function () {
        $.ajax({
            url: '/api/class/save',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(new classViewModel(self.classId(), self.className(), self.classTeacher(), self.classLocation())),
            error: function (response) {
                alert(response.responseJSON["Message"]);
            },
            success: function (data) {
                $("#classModal").modal('hide');
                self.getClassess();
            }
        });
    }

    self.cancel = function () {
        $("#classModal").modal('hide');
    }

    self.getClassess = function (caller) {
        self.classes.removeAll();

        $.ajax({
            url: '/api/class/classes',
            cache: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: {},
            success: function (data) {
                $.each(data, function (key, value) {
                    self.classes.push(new classViewModel(value.Id, value.Name, value.Teacher, value.Location));
                });
            }
        });
    }

}

function studentViewModel(id, firstName, lastName, classId, age, gpa) {
    var self = this;
    self.Id = id;
    self.FirstName = firstName;
    self.LastName = lastName;
    self.ClassId = classId;
    self.Age = age;
    self.Gpa = gpa;
    self.FullName = firstName + "  " + lastName;
}

function studentListViewModel() {
    var self = this;

    self.starRatingThresholdForGpa = 3.2
    self.students = ko.observableArray();
    self.Id = ko.observable(0);
    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.ClassId = ko.observable(0);
    self.Age = ko.observable(0);
    self.Gpa = ko.observable(0);
    self.isEdit = ko.observable(false);
    self.selectedClass = ko.observable();

    self.isBrightStudent = function (gpa) {
        return gpa >= self.starRatingThresholdForGpa;
    }
    self.editStudent = function (editStudent) {

        self.Id(editStudent.Id)
        self.FirstName(editStudent.FirstName);
        self.LastName(editStudent.LastName);
        self.Age(editStudent.Age);
        self.Gpa(editStudent.Gpa);
        self.isEdit(true);
        $("#studentModal").modal('show');
    }

    self.getStudentsOnClassSelection = function (selectedClass) {
        self.selectedClass(selectedClass);
        self.ClassId(selectedClass.Id);
        self.getStudents(selectedClass.Id);
    }

    self.getStudents = function (classId) {

        self.students.removeAll();

        $.ajax({
            url: '/api/student/students/' + classId,
            cache: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: {},
            success: function (data) {
                $.each(data, function (key, value) {
                    self.students.push(new studentViewModel(value.Id, value.FirstName, value.LastName, value.ClassId, value.Age, value.Gpa));
                });
            }
        });

    }

    self.saveStudent = function () {
        $.ajax({
            url: '/api/student/save',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(new studentViewModel(self.Id(), self.FirstName(), self.LastName(), self.ClassId(), self.Age(), self.Gpa())),
            error: function (response) {
                alert(response.responseJSON["Message"]);
            },
            success: function (data) {
                $("#studentModal").modal('hide');
                self.getStudents(self.ClassId());
            }
        });
    }

    self.removeStudent = function (removeStudent) {
        var isConfirmed = confirm("Are you sure want to delete student?");

        if (isConfirmed) {
            $.ajax({
                url: '/api/student/remove/' + removeStudent.Id,
                cache: false,
                type: 'POST',
                error: function (response) {
                    alert("error occurred while deleting class.");
                },
                success: function (data) {
                    self.getStudents(removeStudent.ClassId);
                }
            });
        }
    }

    self.cancel = function () {
        $("#studentModal").modal('hide')
    }

    self.addStudent = function () {
        self.isEdit(false);
        self.Id(0);
        self.FirstName("");
        self.LastName("");
        self.Age(0);
        self.Gpa(0);
    }
}

schoolViewModel = { classListViewModel: new classListViewModel(), studentListViewModel: new studentListViewModel() };
// load student data
schoolViewModel.classListViewModel.getClassess();


$(document).ready(function () {

    // bind view model to referring view
    ko.applyBindings(schoolViewModel);

});