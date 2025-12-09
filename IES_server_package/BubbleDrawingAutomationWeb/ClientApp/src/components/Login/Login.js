import React from 'react';
import { Button, Container, Row, Col, Input, Form, Label, FormGroup } from 'reactstrap';
import './login.css';
import Image from '../Common/Image';
import * as Constants from '../Common/constants'
import useStore from "../Store/store";
import { useNavigate } from "react-router-dom";
import initialState from "../Store/init";

export default function Login() {

    const navigate = useNavigate();
    let state = useStore.getState();
    console.log(state)
    const  handleSubmit1 = (e) => {
        e.preventDefault();
        useStore.setState({ ...initialState, ...state, user: [{}] });
        navigate("/");
    };

 
        return (
            <Container className="my-5 gradient-form ">

                <Row>

                    <Col col='6' className="mb-5 mt-5">
                        <div className="d-flex flex-column ms-5">

                            <div className="text-center">
                                <Image name='halliburton-logo.svg' alt={Constants.APP_NAME} title={Constants.APP_NAME} className="gb_Hc" />
                                <h4 className="mt-1 mb-5 pb-1">INSPECTION EXPERT SYSTEM</h4>
                            </div>

                            
                            <Container >
                                <Label className="d-none" >Please login to your account</Label>

                            <Form autoComplete="off" className="container">
                                    <FormGroup className="d-none">
                                        <Label>User Name / E-Mail</Label>
                                        <Input placeholder='User Name / E-Mail' id='eid' autoComplete="off" defaultValue="***********************" name="eid" type='text' />
                                </FormGroup>
                                    <FormGroup className="d-none">
                                        <Label>Password</Label>
                                        <Input placeholder='Password' id='password' autoComplete="off" defaultValue="**********" name="password" type='password' />
                                </FormGroup>

                                <div className="text-center pt-1 mb-5 pb-1">
                                        <Button type="button"
                                            onClick={handleSubmit1}
                                            className="mb-4 w-10 gradient-custom-2"
                                        >Enter</Button>
                                
                                </div>

                            </Form>
                            </Container>
                        </div>

                    </Col>

                    <Col col='6' className="mb-5">
                        <div className="d-flex flex-column  justify-content-center gradient-custom-2 h-100 mb-4">

                            <div className="text-white px-3 py-4 p-md-5 mx-md-4">
                                <h4 className="mb-4">We are more than just a company</h4>
                                <p className="small mb-0">
                                    <video muted="" id="video" width={400} poster="https://cdn.brandfolder.io/IQ55YUSL/at/q5v89wff9wxr2svt6v4gg7gc/floating_beads.auto" loop="" autoPlay="">
                                        <source src="https://cdn.brandfolder.io/IQ55YUSL/as/3nm8j69k7b77nbfqhrgzrf/Streaks_20s.webm?position=2" type="video/mp4" />
                                            Your browser does not support the html video tag.
                                    </video>
                                </p>
                            </div>

                        </div>

                    </Col>

                </Row>

            </Container>
        );
    }

