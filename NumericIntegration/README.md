# LandauDistribution - NumericIntegration

Since the convergence is different, it is necessary to use different equations for positive and negative &lambda;.

## &lambda; &geq; 0

Since p(&lambda;) has periodicity due to sin, this is used.

![integrate px 1](../figures/integrate_px_1.svg)  

![integrad px](../figures/integrand_px.svg)  

If evaluated here for each cycle of sin, the integration point can always be positive.

![integrate px 2](../figures/integrate_px_2.svg)  
![integrate px 3](../figures/integrate_px_3.svg)  

Evaluate the peak point of the integrated function. It can be seen that t &lt; 1 always.

![integrate px 4](../figures/integrate_px_4.svg)  

Evaluate the upper bound.

![integrate px 7](../figures/integrate_px_7.svg)  
![integrate px 5](../figures/integrate_px_5.svg)  
![integrate px 6](../figures/integrate_px_6.svg)  

Similarly, a lower bound is required.

![integrate px 8](../figures/integrate_px_8.svg)  

## &lambda; &lt; 0

For negative &lambda;, do a variable transformation and use an equation with small oscillations that decay rapidly.  

![integrate nx 1](../figures/integrate_nx_1.svg)  

![integrad nx](../figures/integrand_nx.svg)  

When &lambda; &rarr; -&infin;, the integral J converges to 1.

![integrate nx 3](../figures/integrate_nx_3.svg)  
![integrate nx 2](../figures/integrate_nx_2.svg)  

Evaluate the upper bound.

![integrate nx 4](../figures/integrate_nx_4.svg)  

Also, variable transformation is performed so that the integral interval is finite.  

![integrate nx 5](../figures/integrate_nx_5.svg)  
![integrate nx 6](../figures/integrate_nx_6.svg)  

### Reference
[W.Börsch-Supan, "On the Evaluation of the Function &Phi;(&lambda;) for Real Values of &lambda;" (1961)](https://nvlpubs.nist.gov/nistpubs/jres/65B/jresv65Bn4p245_A1b.pdf)

## Result
[PDF Precision 64](../results/integrate_pdf_precision64.csv)  
[CDF Precision 64](../results/integrate_cdf_precision64.csv)  
[PDF Precision 64(test)](../results/integrate_pdf_precision64_test.csv)  