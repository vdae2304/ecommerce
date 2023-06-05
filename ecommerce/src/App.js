import './App.css';
import { Component } from 'react';
import { Product } from './components/product';
import { SearchBar, Filters } from './components/filters';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      brands: [],
      categories: [],
      products: [],
      filters: {
        q: "",
        category: "",
        brand: "",
        minPrice: "",
        maxPrice: ""
      }
    };
  }

  componentDidMount() {
    this.fetchData("products");
    this.fetchData("categories");
    this.fetchData("brands");
  }

  fetchData = (field, queryParams = null) => {
    let url = `http://localhost:3000/api/v1/${field}`;
    if (queryParams) {
      url += `?${queryParams}`;
    }
    console.log(url);
    fetch(url)
      .then((response) => response.json())
      .then((data) => this.setState((prevState) => {
        prevState[field] = data;
        return prevState;
      }))
      .catch((error) => console.warn(error));
  }

  onChangeHandler = (event) => {
    this.setState((prevState) => {
      prevState.filters[event.target.name] = event.target.value;
      return prevState;
    });
  }

  onSubmitHandler = (event) => {
    const queryParams = new URLSearchParams(this.state.filters).toString();
    this.fetchData("products", queryParams);
    event.preventDefault();
  }

  render() {
    return (
      <div>
        <SearchBar onChangeHandler={this.onChangeHandler}
          onSubmitHandler={this.onSubmitHandler} />
        <div className="layout">
          <Filters categories={this.state.categories}
            brands={this.state.brands}
            onChangeHandler={this.onChangeHandler} />
          <div className="content">
            <h2>Resultados</h2>
            <div className="product-list">
              {this.state.products.map(
                (product) => <Product key={product.sku}
                  sku={product.sku}
                  name={product.name}
                  price={product.price}
                  category={product.category}
                  brand={product.brand}
                  image={product.image}
                  url="" />)}
            </div>
          </div>
        </div>
      </div>
    );
  }
};

export default App;
