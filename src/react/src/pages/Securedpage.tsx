import React, { useState } from 'react';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

const Secured = () => {

  const [forecasts, setForecasts] = useState<WeatherForecast[]>([]);

  React.useEffect(() => {
      (async () => {
      try {
          const response = await fetch(`${process.env.REACT_APP_BACKEND_URL}/weatherforecast`);
          setForecasts(await response.json());
      } catch (e) {
          console.error(e);
      }
      })();
  }, []);




 return (
   <div>

    <h1 id="tableLabel">Weather forecast</h1>

    <p>This component demonstrates fetching data from the server.</p>

     { forecasts.length ? (
      <table className='table table-striped' aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map((forecast) => (
            <tr>
              <td>{ forecast.date }</td>
              <td>{ forecast.temperatureC }</td>
              <td>{ forecast.temperatureF }</td>
              <td>{ forecast.summary }</td>
            </tr>
          ))}
        </tbody>
      </table>
    ): 
    (<p><em>Loading...</em></p>) 
    }
   </div>
 );
};

export default Secured;