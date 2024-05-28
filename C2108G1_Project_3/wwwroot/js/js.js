let myForm = document.getElementById("myForm");
let allInputs = myForm.getElementsByTagName("input");
myForm.onsubmit = onSubmit;
var pass = "123456";
var acount = "admin";
function onSubmit() {
    console.log(pass);
    for (let index = 0; index < allInputs.length; index++) {
        allInputs[index].setCustomValidity("");
    }
    let user = allInputs["username"];
    if (user.value == acount ){
       let pwd = allInputs["pw"];
            if(pass != pwd.value){
                alert("Password not mach")
            } else {
                alert("Login success");
            }
    } else {
        alert("Acount not mach");
    }
    return false;
}
