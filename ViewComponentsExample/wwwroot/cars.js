document.querySelector("#load-cars-button").addEventListener("click", async function () {
    var response = await fetch("load-cars", { method: "GET" });
    var responseBody = await response.text();

    document.querySelector('#list').innerHTML = responseBody;
});