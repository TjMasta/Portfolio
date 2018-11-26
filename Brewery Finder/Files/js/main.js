let searchField = document.querySelector("#searchTerm");
const lastSearch = "txd7163-search";
const storedSearch = localStorage.getItem(lastSearch);

if(storedSearch)
    {
        searchField.value = storedSearch;
    }

searchField.onchange = e=>{ localStorage.setItem(lastSearch, e.target.value); };

window.onload = (e) => {document.querySelector("#search").onclick = getData};

let displayTerm = "";

//Gets the link to the api for the specified info
function getData()
{
    const BREW_URL = "https://api.openbrewerydb.org/breweries?";
    
    let url = BREW_URL;
    
    let city = document.querySelector("#searchTerm").value;
    displayTerm = city;
    
    city = city.trim();
    city = encodeURIComponent(city);
    
    let state = document.querySelector("#stateSelector").value;
        
    let type = document.getElementsByName("type");
    
    if(city.length > 0)
        {
            url = url + "by_city=" + city;
            if(state != "")
            {
                url = url + "&by_state=" + state;
            }
            for(let i = 0; i < type.length; i++)
            {
                if(type[i].checked)
                    url = url + "&by_type=" + type[i].value;
            }
        }
    else if(state != "")
        {
            url = url + "by_state=" + state;
            for(let i = 0; i < type.length; i++)
            {
                if(type[i].checked)
                    url = url + "&by_type=" + type[i].value;
            }
        }
    else
        {
            for(let i = 0; i < type.length; i++)
            {
                if(type[i].checked && type[i].value != "")
                    url = url + "by_type=" + type[i].value;
            }
            if(url == BREW_URL)
                return;
        }
    url = url + "&per_page=50";
    
    document.querySelector("#results").innerHTML = "<b>Searching for " + displayTerm + ".</b>";
    
    $.ajax({
            dataType: "json",
            url: url,
            data: null,
            success: jsonLoaded
        });
}

// Gets the results from the api and displays the name of the brewery with the link to their site, and provides the city and state below.
function jsonLoaded(obj)
{   
    if(obj.length == 0)
              {
                  document.querySelector("#results").innerHTML = `<p><i>No Results found for '${displayTerm}'</i></p>`;
                  $("#results").fadeIn(500);
                  return;
              }
          
          let results = obj;
          let bigString = "<p><i>Here are " + results.length + " results for " + displayTerm + "</i></p>";
          
          for(let i = 0; i < results.length; i++)
              {
                  let result = results[i];
                  
                  let url = result.url;
                  
                  let line = "<a href=" + result.website_url + ">" + result.name + "</a><br>";
                  line = line + "<i>" + result.city + ", " + result.state + "</i><br><br>";
                  
                  bigString += line;
              }
          
          document.querySelector("#results").innerHTML = bigString;
          
          $("#results").fadeIn(500);
}