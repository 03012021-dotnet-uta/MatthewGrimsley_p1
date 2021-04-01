let storeListElement = document.getElementById("store_list");
let stores = new Object();

// fetch all of the stores
fetch(`thestore/stores`)
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
    stores = responseJSON;
    addStoresToHTML(stores);
})
.catch(handleError)

function handleError(error)
{
    console.log(error);
    switch (error.message) {
        case '400':
            console.log("Failed to retrieve stores, request malformed!");
            break;
        case '404':
            console.log("Failed to retrieve stores, resource not found!");
            break;
        default:
            console.log("Failed to retrieve stores, unknown error!");
    }
}

function addStoresToHTML(stores)
{
    let defaultStore = getCookieValue('defaultstore');
    // Create a store element for each store
    stores.forEach(store => {
        let defaultStoreMarker = "";
        if(store.number == defaultStore)
        {
            defaultStoreMarker = "Default";
        }

        let storeDiv = document.createElement("div");
        storeDiv.addEventListener("click", selectStore);
        storeDiv.storeNumber = store.number;
        storeDiv.setAttribute("id", "store_div");
        storeDiv.setAttribute("class", "button");
        storeDiv.innerHTML += ` <span class="left pad_right spaced">${store.name}</span><span class="right pad_left spaced">${store.number}</span><br>
                                <div class="vertical_spacer_half"></div>
                                <span class="left spaced">${store.streetAddress}</span><br>
                                <div class="vertical_spacer_half"></div>
                                <span class="left pad_right spaced">${store.city},</span><span class="left pad_right spaced">${store.stateName}, 
                                </span><span class="left spaced">${store.zipCode}</span><span class="right spaced">${defaultStoreMarker}</span><br>`;
        storeListElement.appendChild(storeDiv);

        if(store.number != defaultStore)
        {
            let setDefaultDiv = document.createElement("div");
            setDefaultDiv.addEventListener("click", setDefaultStore);
            setDefaultDiv.storeNumber = store.number;
            setDefaultDiv.setAttribute("id", "set_default_div");
            setDefaultDiv.setAttribute("class", "button");
            setDefaultDiv.innerHTML += `<span class="left">Make Default Store</span><br>
                                        <div class="vertical_spacer_half"></div>`;
            storeListElement.appendChild(setDefaultDiv);
        }

        let verticalSpacer = document.createElement("div");
        verticalSpacer.setAttribute("class", "vertical_spacer");
        storeListElement.appendChild(verticalSpacer);
    });
}

function selectStore(event)
{
    // Climb the node tree until the div that contains the storeNumber property is reached
    let target = event.target;
    while (target.getAttribute('id') != 'store_div') {
        target = target.parentNode;
    }

    setCookieValue('currentstore', target.storeNumber);
    location = 'fruitestand.html';
}

function setDefaultStore(event)
{
    // Climb the node tree until the div that contains the storeNumber property is reached
    let target = event.target;
    while (target.getAttribute('id') != 'set_default_div') {
        target = target.parentNode;
    }

    fetch(`thestore/store/default/${target.storeNumber}`, {
        method: 'POST'
    })
    .then(response => {
        if(!response.ok)
        {
            throw Error(response.status);
        }
        setCookieValue('defaultstore', target.storeNumber);
        storeListElement.innerHTML = "";
        addStoresToHTML(stores);
    })
    .catch(error => {
        console.log(`${error} Failure to set default store, token invalid.`);
    });
}