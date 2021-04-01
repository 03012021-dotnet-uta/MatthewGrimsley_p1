if(getCookieValue('currentstore') == undefined)
{
    location = 'selectstore.html';
}

let productListElement = document.getElementById("product_list");
let products = new Object();
let inventory = new Object();

// fetch all of the products
fetch(`thestore/products`)
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
    products = responseJSON;
})
.catch(handleProductError)

function handleProductError(error)
{
    console.log(error);
    switch (error.message) {
        case '400':
            console.log("Failed to retrieve products, request malformed!");
            break;
        case '404':
            console.log("Failed to retrieve products, resource not found!");
            break;
        default:
            console.log("Failed to retrieve products, unknown error!");
    }
}

// fetch the selected store's inventory
let storeNumber = getCookieValue('currentstore');
fetch(`thestore/store/${storeNumber}/inventory`)
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
    inventory = responseJSON;
    addProductsToHTML();
})
.catch(handleInventoryError)

function handleInventoryError(error)
{
    console.log(error);
    switch (error.message) {
        case '400':
            console.log("Failed to retrieve inventory, request malformed!");
            break;
        case '404':
            console.log("Failed to retrieve inventory, resource not found!");
            break;
        default:
            console.log("Failed to retrieve inventory, unknown error!");
    }
}

function addProductsToHTML()
{
    console.log(products);
    console.log(inventory);
    debugger;
    // Create a product element for each product
    products.forEach(product => {
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