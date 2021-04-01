// Check for the user's current store
let currentStore = getCookieValue('currentstore');
let username = getCookieValue('username');

if(typeof username == 'undefined')
{
    location='login.html';
}
else
{
    if(username == '')
    {
        location='login.html';
    }
    else
    {
        if(typeof currentStore == 'undefined')
        {
            location='selectstore.html';
        }
        else
        {
            location='fruitestand.html';
        }
    }
}