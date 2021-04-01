function logIn(username, token, permissions, defaultstore)
{
    // Create Session Cookies
    setCookieValue('username', username);
    setCookieValue('token', token);
    setCookieValue('permissions', permissions);
    setCookieValue('currentstore', defaultstore);
    setCookieValue('defaultstore', defaultstore);
}

function logOut()
{
    deleteCookie('username');
    deleteCookie('token');
    deleteCookie('permissions');
    deleteCookie('currentstore');
    deleteCookie('defaultstore');
    location = 'index.html';
    fetch()
}

function getCookie(keyName)
{
    return document.cookie.split('; ').find(row => row.startsWith(`${keyName}=`));
}

function setCookieValue(keyName, value, age_seconds=86400)
{
    document.cookie = `${keyName}=${value};Samesite=strict;Max-age=${age_seconds}`;
}

function getCookieValue(keyName)
{
    let wholeCookie = getCookie(keyName);
    if(typeof wholeCookie == 'undefined')
    {
        return undefined;
    }
    cookieValue = wholeCookie.split('=')[1];
    return cookieValue;
}

function deleteCookie(keyName)
{
    let wholeCookie = getCookie(keyName);
    if(typeof wholeCookie == 'undefined')
    {
        return;
    }
    else
    {
        document.cookie = `${keyName}=;Samesite=strict;Max-age=-1`;
        return;
    }
}