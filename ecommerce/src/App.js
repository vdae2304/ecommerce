import searchIcon from './search-icon.png';
import './App.css';

import { Component } from 'react';

class SearchBar extends Component {
  render() {
    return (
      <div className="header-container">
        <form className="search-form"
              onSubmit={this.props.onSubmitHandler}>
          <input className="search-bar"
                 name="q"
                 type="text"
                 placeholder="Buscar producto"
                 onChange={this.props.onChangeHandler}/>
          <input className="search-button"
                 type="image"
                 alt="Search"
                 src={searchIcon}/>
        </form>
      </div>
    );
  }
};

class Filters extends Component {
  render() {
    return (
      <form className="filters-container">
        <label className="filters-field">Categoría:</label>
        <input className="filters-input"
               name="category"
               type="text"
               list="categories"
               onChange={this.props.onChangeHandler}/>
        <datalist id="categories">
          {this.props.categories.map(
            (category, index) => <option key={index} value={category.name} />
          )}
        </datalist>
        <label className="filters-field">Marca:</label>
        <input className="filters-input"
               name="brand"
               type="text"
               list="brands"
               onChange={this.props.onChangeHandler}/>
        <datalist id="brands">
          {this.props.brands.map(
            (brand, index) => <option key={index} value={brand.name} />
          )}
        </datalist>
        <label className="filters-field">Precio:</label>
        <input className="filters-price"
               name="minPrice"
               type="number"
               placeholder="$ Mínimo"
               step="0.01"
               onChange={this.props.onChangeHandler}/>
        <input className="filters-price"
               name="maxPrice"
               type="number"
               placeholder="$ Máximo"
               step="0.01"
               onChange={this.props.onChangeHandler}/>
      </form>
    );
  }
};

class Product extends Component {
  render() {
    return (
      <div className='product-container'>
        <a href={this.props.url} className='product-image-container'>
          <img alt={this.props.name}
              className='product-image'
              src={this.props.image}/>
        </a>
        <div className='product-info-container'>
          <a className='product-title' href={this.props.url}>
            {this.props.name}
          </a>
          <p className='product-price'>${this.props.price}</p>
          <p className='product-metadata'>Categoría: </p>
          <p className='product-metadata-value'>{this.props.category}</p>
          <p className='product-metadata'>Marca: </p>
          <p className='product-metadata-value'>{this.props.brand}</p>
        </div>
      </div>
    );
  }
};

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
                   onSubmitHandler={this.onSubmitHandler}/>
        <div className="layout">
          <Filters categories={this.state.categories}
                   brands={this.state.brands}
                   onChangeHandler={this.onChangeHandler}/>
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
