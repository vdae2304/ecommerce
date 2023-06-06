import searchIcon from './search-icon.png';
import './search-bar.css';
import { Component } from 'react';

class SearchBar extends Component {
  constructor(props) {
    super(props);
    this.state = {
      q: ""
    };
  }

  onChangeHandler = (event) => {
    this.setState({ q: event.target.value });
  }

  onSubmitHandler = (event) => {
    const queryParams = new URLSearchParams(this.state);
    window.location.href = `/?${queryParams.toString()}`;
    event.preventDefault();
  }

  render() {
    return (
      <form className="search-bar" onSubmit={this.onSubmitHandler}>
        <input className="search-input"
               name="q"
               type="text"
               placeholder="Buscar producto"
               onChange={this.onChangeHandler} />
        <input className="search-button"
          type="image"
          alt="Search"
          src={searchIcon} />
      </form>
    );
  }
};

export default SearchBar;
