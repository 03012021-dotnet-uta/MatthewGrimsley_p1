if(getCookieValue('currentstore') == undefined)
{
    location = 'selectstore.html';
}

let productListElement = document.getElementById("product_list");
let products = new Object();

// fetch product list that includes the selected store's inventory
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
    products = responseJSON;
    addProductsToHTML();
})
.catch(handleError)

function handleError(error)
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

function addProductsToHTML()
{
    // Create a product element for each product
    products.forEach(product => {
        let productDiv = document.createElement("div");
        productDiv.setAttribute("id", "product_div");
        productDiv.innerHTML += ` <span class="flex_item">
                                    <span class="border">
                                        <img src="images/${product.imageLink}"><br>
                                        <div class="info">
                                            <span class="left pad_right">${product.partName} </span>
                                                <a class="left pad_left" href="${product.imageCredit}">Img</a>
                                                <span class="right">$${product.unitPrice}/${product.unitOfMeasure}</span><br>
                                            <span class="left pad_right">${product.partDescription}</span><br>
                                            <span class="left pad_right">Available:${product.inventory} ${product.unitOfMeasure}</span>
                                                <input id="${product.partNumber}qty" class="quantity" type="number" min="0" max="${product.inventory}">
                                                <span class="right button" onclick="addToCart(${product.partNumber});">Add to Cart</span><br>
                                            <span class="vertical_spacer"></span>
                                        </div>
                                    </span>
                                </span>`;
        productListElement.appendChild(productDiv);

        let verticalSpacer = document.createElement("div");
        verticalSpacer.setAttribute("class", "vertical_spacer");
        productListElement.appendChild(verticalSpacer);
    });
}

function addToCart(partNumber)
{
    let quantityElement = document.getElementById(`${partNumber}qty`);

    let quantity = parseInt(quantityElement.value);
    console.log(quantity);
    if(!quantityElement.reportValidity())
    {
        console.log("Invalid quantity while adding to Cart")
        return;
    }
    if(isNaN(quantity))
    {
        console.log("Invalid quantity while adding to Cart")
        quantityElement.setCustomValidity('Please enter a quantity first!');
        quantityElement.reportValidity();
        return;
    }
    localStorage.setItem(`${partNumber}`, `${quantity}`);
}