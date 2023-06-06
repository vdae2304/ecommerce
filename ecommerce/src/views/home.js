import './home.css';
import { Component } from 'react';
import { useSearchParams } from 'react-router-dom';

import Logo from '../components/header/logo';
import SearchBar from '../components/header/search-bar';
import ShoppingCart from '../components/header/shopping-cart';

import Filters from '../components/filters';
import ProductCard from '../components/product-card';
import NavigationControls from '../components/navigation-controls';

class Home extends Component {
  constructor(props) {
    super(props);
    this.state = {
      brands: [],
      categories: [],
      products: [],
      page: 1,
      filters: {}
    };
  }

  componentDidMount() {
    this.fetchData("products");
    this.fetchData("categories");
    this.fetchData("brands");
  }

  fetchData = (field, queryParams = null) => {
    let url = `/api/v1/${field}`;
    if (queryParams) {
      url += `?${queryParams.toString()}`;
    }
    console.log(`Fetching data from ${url}`);
    fetch(url)
      .then((response) => response.json())
      .then((data) => this.setState((prevState) => {
        prevState[field] = data;
        return prevState;
      }))
      .catch((error) => console.error(error));
  }

  onChangeHandler = (event) => {
    this.setState((prevState) => {
      prevState.filters[event.target.name] = event.target.value;
      return prevState;
    });
  }

  onSubmitHandler = (event, page = 1) => {
    const queryParams = new URLSearchParams({
      ...this.state.filters,
      offset: 50*(page - 1)
    });
    this.setState({...this.state, page: page});
    this.fetchData("products", queryParams);
    event.preventDefault();
  }

  render() {
    return (
      <div>
        <div className="header-container">
          <Logo/>
          <SearchBar onChangeHandler={this.onChangeHandler}
                     onSubmitHandler={this.onSubmitHandler}/>
          <ShoppingCart value="0"/>
        </div>
        <div className="layout">
          <Filters categories={this.state.categories}
                   brands={this.state.brands}
                   onChangeHandler={this.onChangeHandler}
                   onSubmitHandler={this.onSubmitHandler}/>
          <div className="content">
            <h1 className="product-search-title">Resultados</h1>
            <div className="product-search-results">
              {this.state.products.map((product) =>
                <ProductCard key={product.sku}
                             sku={product.sku}
                             name={product.name}
                             price={product.price}
                             category={product.category}
                             brand={product.brand}
                             image={product.image}
                             url={`/product/${product.sku}/`} />)}
            </div>
          </div>
        </div>
        <NavigationControls page={this.state.page}
                            lastPage="10"
                            onSubmitHandler={this.onSubmitHandler}/>
      </div>
    );
  }
};

export default (props) => <Home {...props} q={useSearchParams()[0]} />;
