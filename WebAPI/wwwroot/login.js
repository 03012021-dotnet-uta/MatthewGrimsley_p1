let usernameElement = document.getElementById("username");
let passwordElement = document.getElementById("password");
let submitButton = document.getElementById("submit_button");

usernameElement.value = "";
passwordElement.value = "";
submitButton.addEventListener("click", login);

function login() {
    let username = getUsername();
    let password = getPassword();

    if(username === false)
    {
        return;
    }
    if(password === false)
    {
        return;
    }

    console.log("attempting to log in: " + username + ", " + password);

    fetch(`thestore/login/${username}`)
    .then(response => {
        if(!response.ok)
        {
            throw Error(response.status);
        }
        else
        {
            return response.json();
        }
    })
    .then(responseJSON => {
        logIn(responseJSON.username, responseJSON.token, responseJSON.permissions, responseJSON.defaultStore);
        // Redirect to appropriate page
        switch (responseJSON.permissions) {
            case 1:
                if(responseJSON.defaultStore == 0)
                {
                    location = 'selectstore.html';
                }
                else
                {
                    location = 'fruitestand.html';
                }
                
                break;
            case 2:
            case 3:
                location = 'storemenu.html';
                break;
            default:
                console.log(`Invalid user permissions level = ${responseJSON.permissions}`);
                break;
        }
    })
    .catch(handleError)
}

function handleError(error)
{
    console.log(error);
    switch (error.message) {
        case '400':
            console.log("Login failure, request malformed!");
        case '401':
            console.log("Login failure, password incorrect");
            alert("username/password invalid!");
            break;
        case '404':
            console.log("Login failure, username not found.");
            alert("username/password invalid!");
            break;
        default:
            console.log("Unknown login error!");
    }
    clearFields();
}

function getUsername() {
    let username = usernameElement.value;
    if((typeof username) != "string")
    {
        username = '';
        usernameElement.setCustomValidity("Please enter your username");
        usernameElement.reportValidity();
        console.log("username validation failed.");
        return false;
    }
    username = username.trim();
    if(username.length < 6)
    {
        usernameElement.setCustomValidity("Username must be at least 6 characters");
        usernameElement.reportValidity();
        console.log("username too short.");
        return false;
    }
    return username;
}

function getPassword() {
    let password = passwordElement.value;
    if((typeof password) != "string")
    {
        password = '';
        passwordElement.setCustomValidity("Please enter your password");
        passwordElement.reportValidity();
        console.log("password validation failed.");
        return false;
    }
    password = password.trim();
    return password;
}

function clearFields()
{
    usernameElement.value = "";
    passwordElement.value = "";
}