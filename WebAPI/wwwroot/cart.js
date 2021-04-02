const maxSubtotal = 250;

let productListElement = document.getElementById("product_list");
let warningElement = document.createElement("div");
let products = new Object();
let totalDiv;
let subtotalPrice = 0;
let tax = 0;
let totalPrice = 0;

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
    listParts(products);
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

function listParts(products)
{
    subtotalPrice = 0;
    products.forEach(product => {
        listPart(product);
    });
    
    let taxPercentage = getCookieValue('taxRate');
    tax = subtotalPrice * taxPercentage / 100;
    totalPrice = subtotalPrice + tax;

    totalDiv = document.createElement("div");
    totalDiv.innerHTML += ` <hr><div><span class="right pad_right">$${subtotalPrice.toFixed(2)}</span><span class="right pad_right">Subtotal:</span></div><br>
                            <div class="vertical_spacer_half"></div>
                            <div><span class="right pad_right">$${tax.toFixed(2)}</span><span class="right pad_right">Tax:</span></div><br>
                            <div class="vertical_spacer_half"></div>
                            <div><span class="right pad_right">$${totalPrice.toFixed(2)}</span><span class="right pad_right">Total:</span></div><br>
                            <div class="vertical_spacer"></div>`;
    productListElement.appendChild(totalDiv);

    // Create Checkout Button
    let checkoutButton = document.createElement('span');
    checkoutButton.textContent = "Checkout";
    checkoutButton.setAttribute("class", "button right pad_right");
    checkoutButton.addEventListener("click", checkOut);
    productListElement.appendChild(checkoutButton);
    productListElement.appendChild(warningElement);
}

function listPart(product)
{
    let quantity = localStorage.getItem(`${product.partNumber}`);
    if(quantity == null)
    {
        return;
    }
    quantity = parseInt(quantity);

    if(quantity == 0)
    {
        return;
    }

    if(quantity > product.inventory)
    {
        quantity = product.inventory;
        localStorage.setItem(`${product.partNumber}`, quantity);
    }

    let productDiv = document.createElement("div");
    productDiv.setAttribute("id", `${product.partNumber}div`);

    let lineTotal = product.unitPrice * quantity;
    subtotalPrice += lineTotal;
    productDiv.innerHTML += `   <div><span class="left pad_right">${product.partName}</span><span class="right pad_right">$${lineTotal.toFixed(2)}</span>
                                <span class="right pad_right">&nbsp;&nbsp;=&nbsp;</span>
                                <input id="${product.partNumber}qty" class="right quantity" onChange="updateQuantity(${product.partNumber})"
                                    type="number" value="${quantity}" min="0" max="${product.inventory}">
                                <span class="right">$${product.unitPrice}/${product.unitOfMeasure}&nbsp;&nbsp;x&nbsp;&nbsp;</span></div><br>
                                <div><span class="left pad_left">${product.partDescription}</span><br></div>
                                <span class="vertical_spacer"></span>`;
    productListElement.appendChild(productDiv);
}

function updateQuantity(partNumber)
{
    let quantityElement = document.getElementById(`${partNumber}qty`);
    let productDiv = document.getElementById(`${partNumber}div`);

    quantity = parseInt(quantityElement.value);
    if(!quantityElement.reportValidity())
    {
        console.log("Invalid quantity in Cart")
        return;
    }
    if(isNaN(quantity))
    {
        console.log("Invalid quantity in Cart")
        quantityElement.setCustomValidity('Please enter a valid quantity');
        quantityElement.reportValidity();
        return;
    }

    localStorage.setItem(`${partNumber}`, quantity);

    location.reload();
}

function checkOut()
{
    // Check for subtotal above
    if(subtotalPrice >= maxSubtotal)
    {
        warningElement.textContent = `Cannot Place Order! Subtotal must be less than $${maxSubtotal}`;
        return;
    }

    // Build a Cart Object
    let cart = new Object();
    cart.storeNumber = parseInt(storeNumber);
    cart.subtotal = subtotalPrice;
    cart.tax = tax;
    cart.total = totalPrice;
    cart.datetime = Date.now();

    let productList = new Array();
    products.forEach(product => {
        let quantityInput = document.getElementById(`${product.partNumber}qty`);
        if(quantityInput != null)
        {
            let cartItem = new Object();
            cartItem.partNumber = product.partNumber;
            cartItem.unitPrice = product.unitPrice;
            cartItem.unitOfMeasure = product.unitOfMeasure;
            cartItem.quantity = parseInt(quantityInput.value);
            productList.push(cartItem);
        }
    });
    cart.productList = productList;

    console.log(cart);
    
    // Send the request to create an order
    fetch('thestore/placeorder', {
        method: 'POST',
        headers: {
          'Content-Type':'application/json'
        },
        body:JSON.stringify(cart)
    })
    .then(response => {
        if(!response.ok)
        {
            throw Error(response.status);
        }
        else
        {
            clearCart();
            warningElement.textContent = "Checkout Successful, Redirecting...";
            setTimeout(()=> location='fruitestand.html', 3000);
        }
    })
    .catch(error => {
        console.log(error);
        switch (error.message) {
            case '400':
                console.log("Checkout failure, request malformed.")
                break;
            case '403':
                console.log("Checkout failure, denied.")
                warningElement.textContent = "Checkout failed, Server denied request!";
                break;
            default:
                console.log("Unknown error during checkout.");
        }
    });
}

function clearCart()
{
    products.forEach(product => {
        deleteCookie(`${product.partNumber}`);
    });
}