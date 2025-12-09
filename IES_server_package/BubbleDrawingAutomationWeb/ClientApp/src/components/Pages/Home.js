import React, { Component } from 'react';
import Canvas   from '../Canvas/Canvas';

export class Home extends Component {
    static displayName = Home.name;

    render() {
       
            return (
              <>
                    <div className="ballooning">
                        
                        <Canvas ></Canvas>

                    </div>
              </>
            );
        }
}
