import './navigation-controls.css';
import { Component } from 'react';

class NavigationControls extends Component {
  onPreviousPage = (event) => {
    if (this.props.page > 1) {
      this.props.onSubmitHandler(event, this.props.page - 1);
    }
    event.preventDefault();
  }

  onNextPage = (event) => {
    if (this.props.page < this.props.lastPage) {
      this.props.onSubmitHandler(event, this.props.page + 1);
    }
    event.preventDefault();
  }

  render() {
    return (
      <div className="footer">
        <button className="control-buttons" onClick={this.onPreviousPage}>
          {"< Anterior"}
        </button>
        {this.props.page}
        <button className="control-buttons" onClick={this.onNextPage}>
          {"Siguiente >"}
        </button>
      </div>
    );
  }
};

export default NavigationControls;
