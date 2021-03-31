const usernameInput = document.getElementById('username_input');
const firstNameInput = document.getElementById('firstname_input');
const lastNameInput = document.getElementById('lastname_input');
const emailInput = document.getElementById('email_input');
const phoneNumberInput = document.getElementById('phonenumber_input');
const cityInput = document.getElementById('city_input');
const stateInput = document.getElementById('state_input');
const zipInput = document.getElementById('zipcode_input');
const addressInput = document.getElementById('streetaddress_input');
const submitButton = document.getElementById('submitButton');

submitButton.addEventListener('click', createAccount);

// Fetch the list of states from the server and add each one to the
// list of states to the states dropdown
fetch('thestore/states')
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
    responseJSON.forEach(state => {
        stateInput.innerHTML += `<option value='${state.name}'>${state.abbreviation}</option>`;
    });
})
.catch(error => {
    console.log(error);
    switch (error.message) {
        case '400':
            console.log("Failure retrieving states list, request malformed!");
        case '404':
            console.log("Failure retrieving states list, no states found");
            break;
        default:
            console.log("Unknown error while retrieving states list");
    }
});

async function createAccount()
{
    let newaccountdata;

    // Validate the Account Information
    let username = usernameInput.value;
    if(!usernameInput.reportValidity())
    {
        return;
    }
    if(typeof username != 'string')
    {
        usernameInput.setCustomValidity('Please enter a valid username');
        usernameInput.reportValidity();
        return;
    }
    if(username.length < 6)
    {
        usernameInput.setCustomValidity('Username must be atleast 6 characters');
        usernameInput.reportValidity();
        return;
    }
    newaccountdata.username = username;

    let firstname = firstNameInput.value;
    if(!firstNameInput.reportValidity())
    {
        return;
    }
    if(typeof firstname != 'string' || firstname.length == 0)
    {
        firstNameInput.setCustomValidity('Please enter your personal name');
        firstNameInput.reportValidity();
        return;
    }
    newaccountdata.firstname = firstname;

    let lastname = lastNameInput.value;
    if(!lastNameInput.reportValidity())
    {
        return;
    }
    if(typeof lastname != 'string' || lastname.length == 0)
    {
        lastNameInput.setCustomValidity('Please enter your family name');
        lastNameInput.reportValidity();
        return;
    }
    newaccountdata.lastname = lastname;

    let email = emailInput.value;
    if(!emailInput.reportValidity())
    {
        return;
    }
    if(typeof email != 'string' || email.length == 0)
    {
        emailInput.setCustomValidity('Please enter your email address');
        emailInput.reportValidity();
        return;
    }
    newaccountdata.email = email;

    let phonenumber = phoneNumberInput.value;
    if(!phoneNumberInput.reportValidity())
    {
        return;
    }
    if(typeof phonenumber != 'number')
    {
        phoneNumberInput.setCustomValidity('Please enter your phone number');
        phoneNumberInput.reportValidity();
        return;
    }
    newaccountdata.phonenumber = phonenumber;

    let city = cityInput.value;
    if(!cityInput.reportValidity())
    {
        return;
    }
    if(typeof city != 'string' || city.length < 2)
    {
        cityInput.setCustomValidity('Please enter your city');
        cityInput.reportValidity();
        return;
    }
    newaccountdata.city = city;

    let state = state_input.value;
    if(!state_input.reportValidity())
    {
        return;
    }
    newaccountdata.state = state;

    let zipcode = zipInput.value;
    if(!zipInput.reportValidity())
    {
        return;
    }
    newaccountdata.zipcode = zipcode;

    let streetaddress = addressInput.value;
    if(!addressInput.reportValidity())
    {
        return;
    }
    if(typeof streetaddress != 'string' || streetaddress.length == 0)
    {
        addressInput.setCustomValidity('Please enter your family name');
        addressInput.reportValidity();
        return;
    }
    newaccountdata.streetaddress = streetaddress;

    newaccountdata.salt = "temporary";
    newaccountdata.hash = "temporary";

    // Send the request to create a new account
    fetch('thestore/newaccount', {
        method: 'POST',
        body: JSON.stringify(newaccountdata)
    })
    .then(response => {
        if(!response.ok)
        {
            throw Error(response.status);
        }
        else
        {
            submitButton.textContent = "Account Created, Redirecting...";
            setTimeout(()=>location='login.html', 3000);
        }
    })
    .catch(error => {
        console.log(error);
        switch (error.message) {
            case '400':
                console.log("Account creation failure, request malformed!")
            default:
                console.log("Unknown error during account creation");
        }
    });
}
